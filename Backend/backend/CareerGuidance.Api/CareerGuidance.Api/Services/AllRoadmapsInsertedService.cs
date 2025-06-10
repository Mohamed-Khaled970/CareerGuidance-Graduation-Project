/*
* File Name: allRoadmapInsertedService.cs
* Author Information: Abdelrahman Rezk
* Date of Creation: 2024-10-13
* Version Information: v1.0
* Dependencies: Newtonsoft.Json, CareerGuidance.Api.Data.ApplicationDbContext
* Contributors: Abdelrahman Rezk
* Last Modified Date: 2024-10-27
*
* Description:
*      Service for handling roadmap data insertion, retrieval, updating, and deletion in the database.
*/

using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CareerGuidance.Api.Services
{
    public class allRoadmapsInsertedService : IAllRoadmapsInsertedService
    {
        private readonly ApplicationDbContext _context;


        public allRoadmapsInsertedService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Result> AddFilteredRoadmapAsync(allRoadmapInsertedDto request, CancellationToken cancellationToken = default)
        {
            var sharedId = Guid.NewGuid(); // Create shared ID to link filtered roadmap data with main data

            // Deserialize the roadmap data
            var roadmapDto = JsonConvert.DeserializeObject<roadmapDataDto>(request.roadmapData);

            if (roadmapDto == null)
            {
                return Result.Failuer<Roadmap>(RoadmapError.InvalidData);
            }

            // Check if the category exists
            var existingCategory = await _context.RoadmapCategories
                .FirstOrDefaultAsync(c => c.Category == roadmapDto.roadmapCategory, cancellationToken);

            if (existingCategory == null)
            {
                return Result.Failuer<Roadmap>(RoadmapError.CategoryNotFound);
            }

            // Validate roadmap data
            if (string.IsNullOrWhiteSpace(roadmapDto.roadmapName))
            {
                return Result.Failuer<Roadmap>(RoadmapError.RoadmapNameEmpty);
            }

            if (string.IsNullOrEmpty(roadmapDto.roadmapDescription))
            {
                return Result.Failuer<Roadmap>(RoadmapError.DescriptionEmpty);
            }

            if (string.IsNullOrEmpty(roadmapDto.imageUrl))
            {
                return Result.Failuer<Roadmap>(RoadmapError.ImageUrlEmpty);
            }

            // Check if nodes are empty or invalid
            if (roadmapDto.Nodes == null || !roadmapDto.Nodes.Any())
            {
                return Result.Failuer<Roadmap>(RoadmapError.NodesEmpty);
            }

            // Check if edges are empty or invalid
            if (roadmapDto.Edges == null || !roadmapDto.Edges.Any())
            {
                return Result.Failuer<Roadmap>(RoadmapError.EdgesEmpty);
            }

            // Track unique node titles and links
            var uniqueNodes = new HashSet<string>();
            var uniqueLinks = new HashSet<string>();


            foreach (var nodeDto in roadmapDto.Nodes)
            {
                // Validate node title
                if (string.IsNullOrWhiteSpace(nodeDto.Data.Title))
                {

                    return Result.Failuer<Roadmap>(RoadmapError.NodeTitleEmpty);
                }

                // Check for duplicate node titles
                if (!uniqueNodes.Add(nodeDto.Data.Title))
                {
                    return Result.Failuer<Roadmap>(RoadmapError.DuplicateNodeTitle);
                }

                // Check for unique links within the node
                foreach (var linkDto in nodeDto.Data.Links)
                {
                    if (string.IsNullOrWhiteSpace(linkDto.Url))
                    {
                        return Result.Failuer<Roadmap>(RoadmapError.LinkUrlEmpty);
                    }

                    if (!uniqueLinks.Add(linkDto.Url))
                    {
                        return Result.Failuer<Roadmap>(RoadmapError.DuplicateLink);
                    }

                }
            }

            // Get category ID
            var categoryId = existingCategory.Id;

            // Check for existing roadmap with the same name
            var existingRoadmap = await _context.ParsedRoadmaps
                .AnyAsync(r => r.RoadmapName == roadmapDto.roadmapName, cancellationToken);


            if (existingRoadmap)
            {
                return Result.Failuer<Roadmap>(RoadmapError.Dublicated);
            }

            var roadmap = new Roadmap
            {
                RoadmapName = roadmapDto.roadmapName,
                RoadmapDescription = roadmapDto.roadmapDescription,
                Category = roadmapDto.roadmapCategory,
                ImageUrl = roadmapDto.imageUrl,
                Id = sharedId,
                CategoryId = categoryId
            };
            _context.ParsedRoadmaps.Add(roadmap);
            await _context.SaveChangesAsync(cancellationToken);

            var sharedroadmap = new allRoadmapInserted
            {
                Id = sharedId,
                roadmapData = request.roadmapData,
                CategoryId = categoryId,
            };
            _context.Roadmaps.Add(sharedroadmap); // Insert roadmap into the Roadmaps table in the database
            await _context.SaveChangesAsync(cancellationToken);

            foreach (var nodeDto in roadmapDto.Nodes)
            {
                var node = new Node
                {
                    Title = nodeDto.Data.Title,
                    Description = nodeDto.Data.Description,
                    Type = nodeDto.Type,
                    Position = $"x:{nodeDto.Position.X},y:{nodeDto.Position.Y}",
                    RoadmapId = roadmap.Id
                };

                _context.Nodes.Add(node);
                await _context.SaveChangesAsync(cancellationToken);

                foreach (var linkDto in nodeDto.Data.Links)
                {

                    var link = new Link
                    {
                        Type = linkDto.Type,
                        EnOrAr = linkDto.EnOrAr,
                        Title = linkDto.Title,
                        Url = linkDto.Url,
                        NodeId = node.Id
                    };

                    _context.Links.Add(link);
                }
            }


            foreach (var edgeDto in roadmapDto.Edges)
            {
                var edge = new Edge
                {
                    Source = edgeDto.Source,
                    Target = edgeDto.Target,
                    RoadmapId = roadmap.Id
                };

                _context.Edges.Add(edge);
            }


            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }



        public async Task<Result> CheckRoadmapInformationAsync(CreateRoadmapRequest request, CancellationToken cancellationToken = default)
        {

            var existingRoadmapName = await _context.ParsedRoadmaps
                 .AnyAsync(x => x.RoadmapName == request.RoadmapName, cancellationToken);

            if (existingRoadmapName)
                return Result.Failuer(RoadmapError.Dublicated);

            return Result.Success();





        }


        // Retrieves all roadmaps from the database
        public async Task<IEnumerable<allRoadmapInserted>> GetAllAsync()
        {
            return await _context.Roadmaps.ToListAsync();

        }

        // Retrieves a specific roadmap by ID
        public async Task<allRoadmapInserted> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default)
        {
            return await _context.Roadmaps.FindAsync(Id, cancellationToken);
        }



        // Retrieves all roadmaps from the database
        public async Task<IEnumerable<allRoadmapInserted>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Roadmaps.ToListAsync(cancellationToken);
        }









        // Updates an existing roadmap's data based on the provided ID
        public async Task<Result> UpdateAsync(Guid sharedId, allRoadmapInsertedDto request, CancellationToken cancellationToken = default)

        {


            // البحث عن الـ Roadmap باستخدام الـ Shared ID
            var roadmap = await _context.Roadmaps
                .FirstOrDefaultAsync(r => r.Id == sharedId, cancellationToken);

            if (roadmap == null)
            {
                return Result.Failuer(RoadmapError.NotFound);  // رسالة عند عدم العثور على الـ Roadmap
            }

            // Deserialize roadmap data
            var roadmapDto = JsonConvert.DeserializeObject<roadmapDataDto>(request.roadmapData);
            if (roadmapDto == null)
            {
                return Result.Failuer(RoadmapError.InvalidData); // رسالة عند البيانات غير صالحة
            }

            // تحقق من اسم التراك

            if (string.IsNullOrWhiteSpace(roadmapDto.roadmapName))
            {
                return Result.Failuer(RoadmapError.RoadmapNameEmpty); // رسالة عند غياب اسم التراك
            }


            // تحقق من وجود النودز
            if (roadmapDto.Nodes == null || !roadmapDto.Nodes.Any())
            {
                return Result.Failuer(RoadmapError.NodesEmpty); // رسالة عند غياب النودز
            }

            // التحقق من أن الـ Roadmap name غير مكرر
            var isNameExists = await _context.ParsedRoadmaps
                .AnyAsync(r => r.RoadmapName == roadmapDto.roadmapName && r.Id != sharedId, cancellationToken);

            if (isNameExists)
            {
                return Result.Failuer(RoadmapError.Dublicated); // رسالة عند وجود اسم مكرر
            }

            // تحقق من البيانات داخل كل Node
            foreach (var nodeDto in roadmapDto.Nodes)
            {
                if (string.IsNullOrWhiteSpace(nodeDto.Data.Title) || string.IsNullOrWhiteSpace(nodeDto.Data.Description))
                {
                    return Result.Failuer(RoadmapError.NodeTitleEmpty); ; // رسالة عند غياب عنوان أو وصف النود
                }

                foreach (var linkDto in nodeDto.Data.Links)
                {
                    if (string.IsNullOrWhiteSpace(linkDto.Title) || string.IsNullOrWhiteSpace(linkDto.Url))
                    {
                        return Result.Failuer(RoadmapError.LinkUrlEmpty); // رسالة عند غياب عنوان أو URL الرابط
                    }
                }
            }


            // إذا جميع الفحوصات نجحت، نحدث البيانات
            roadmap.roadmapData = request.roadmapData;

            // تحديث الـ Filtered Roadmap المرتبطة
            await UpdateFilterAsync(request, sharedId, cancellationToken);

            // حفظ التعديلات
            _context.Roadmaps.Update(roadmap);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(); // رسالة عند نجاح التحديث
        }




        private async Task UpdateFilterAsync(allRoadmapInsertedDto request, Guid sharedId, CancellationToken cancellationToken = default)

        {
            var roadmapDto = JsonConvert.DeserializeObject<roadmapDataDto>(request.roadmapData);

            // تحديث الـ ParsedRoadmap
            var roadmap = await _context.ParsedRoadmaps
                .FirstOrDefaultAsync(r => r.Id == sharedId, cancellationToken);

            if (roadmap == null)
            {
                throw new KeyNotFoundException("The parsed roadmap with the provided Shared ID does not exist.");
            }

            roadmap.RoadmapName = roadmapDto.roadmapName;
            roadmap.RoadmapDescription = roadmapDto.roadmapDescription;
            roadmap.ImageUrl = roadmapDto.imageUrl;

            _context.ParsedRoadmaps.Update(roadmap);

            // حذف الـ Nodes القديمة
            var existingNodes = _context.Nodes.Where(n => n.RoadmapId == sharedId).ToList();
            foreach (var node in existingNodes)
            {
                var existingLinks = _context.Links.Where(l => l.NodeId == node.Id).ToList();
                _context.Links.RemoveRange(existingLinks);
            }
            _context.Nodes.RemoveRange(existingNodes);

            // إضافة أو تحديث الـ Nodes الجديدة
            foreach (var nodeDto in roadmapDto.Nodes)
            {
                var node = new Node
                {
                    Title = nodeDto.Data.Title,
                    Description = nodeDto.Data.Description,
                    Type = nodeDto.Type,
                    Position = $"x:{nodeDto.Position.X},y:{nodeDto.Position.Y}",
                    RoadmapId = sharedId
                };
                _context.Nodes.Add(node);

                foreach (var linkDto in nodeDto.Data.Links)
                {
                    var link = new Link
                    {
                        Type = linkDto.Type,
                        EnOrAr = linkDto.EnOrAr,
                        Title = linkDto.Title,
                        Url = linkDto.Url,
                        NodeId = node.Id
                    };
                    _context.Links.Add(link);
                }
            }

            // حذف الـ Edges القديمة
            var existingEdges = _context.Edges.Where(e => e.RoadmapId == sharedId).ToList();
            _context.Edges.RemoveRange(existingEdges);

            // إضافة الـ Edges الجديدة
            foreach (var edgeDto in roadmapDto.Edges)
            {
                var edge = new Edge
                {
                    Source = edgeDto.Source,
                    Target = edgeDto.Target,
                    RoadmapId = sharedId
                };
                _context.Edges.Add(edge);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }


        // Deletes a specific roadmap by ID from the database

        //public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)

        //{

        //    // Fetch the roadmap (main entity) by ID
        //    var roadmap = await _context.Roadmaps
        //        .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        //    if (roadmap == null)
        //        return Result.Failuer(RoadmapError.NotFound); // Return false if the roadmap doesn't exist

        //    // Fetch the related parsed roadmap with its entities
        //    var parsedRoadmap = await _context.ParsedRoadmaps
        //        .Include(pr => pr.Nodes) // Include related nodes
        //            .ThenInclude(n => n.Links) // Include links within nodes
        //        .Include(pr => pr.Edges) // Include edges
        //        .FirstOrDefaultAsync(pr => pr.Id == id, cancellationToken);

        //    if (parsedRoadmap != null)
        //    {
        //        // Remove links associated with nodes
        //        var links = parsedRoadmap.Nodes?.SelectMany(n => n.Links).ToList();
        //        if (links?.Any() == true)
        //            _context.Links.RemoveRange(links);

        //        // Remove nodes associated with the parsed roadmap
        //        if (parsedRoadmap.Nodes?.Any() == true)
        //            _context.Nodes.RemoveRange(parsedRoadmap.Nodes);

        //        // Remove edges associated with the parsed roadmap
        //        if (parsedRoadmap.Edges?.Any() == true)
        //            _context.Edges.RemoveRange(parsedRoadmap.Edges);

        //        // Remove the parsed roadmap itself
        //        _context.ParsedRoadmaps.Remove(parsedRoadmap);
        //    }

        //    // Remove the main roadmap
        //    _context.Roadmaps.Remove(roadmap);

        //    // Save changes to the database
        //    await _context.SaveChangesAsync(cancellationToken);

        //    return Result.Success(); // Indicate successful deletion
        //}


        public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // Fetch the roadmap (main entity) by ID
            var roadmap = await _context.Roadmaps
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

            if (roadmap == null)
                return Result.Failuer(RoadmapError.NotFound); // Return false if the roadmap doesn't exist

            // Fetch the related parsed roadmap with its entities
            var parsedRoadmap = await _context.ParsedRoadmaps
                .Include(pr => pr.Nodes) // Include related nodes
                    .ThenInclude(n => n.Links) // Include links within nodes
                .Include(pr => pr.Edges) // Include edges
                .FirstOrDefaultAsync(pr => pr.Id == id, cancellationToken);

            if (parsedRoadmap != null)
            {
                // Remove links associated with nodes
                var links = parsedRoadmap.Nodes?.SelectMany(n => n.Links).ToList();
                if (links?.Any() == true)
                    _context.Links.RemoveRange(links);

                // Remove nodes associated with the parsed roadmap
                if (parsedRoadmap.Nodes?.Any() == true)
                    _context.Nodes.RemoveRange(parsedRoadmap.Nodes);

                // Remove edges associated with the parsed roadmap
                if (parsedRoadmap.Edges?.Any() == true)
                    _context.Edges.RemoveRange(parsedRoadmap.Edges);

                // Remove the parsed roadmap itself
                _context.ParsedRoadmaps.Remove(parsedRoadmap);
            }

            // ✅ Remove related progressBar entries
            var progressBars = await _context.progressBar
                .Where(p => p.RoadmapId == id)
                .ToListAsync(cancellationToken);

            if (progressBars.Any())
                _context.progressBar.RemoveRange(progressBars);

            // Remove the main roadmap
            _context.Roadmaps.Remove(roadmap);

            // Save changes to the database
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(); // Indicate successful deletion
        }


        public async Task<Result> UpdateRoadmapInformationAsync([FromBody] UpdateRoadmapRequest request, CancellationToken cancellationToken = default)
        {
            var existingRoadmapName = await _context.ParsedRoadmaps
                .AnyAsync(x => x.RoadmapName == request.RoadmapName);


            if (existingRoadmapName)
                return Result.Failuer(RoadmapError.Dublicated);

            return Result.Success();
        }
    }
}
