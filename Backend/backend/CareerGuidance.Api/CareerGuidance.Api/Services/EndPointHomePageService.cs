using CareerGuidance.Api.DbContext;
using CareerGuidance.Api.DTO;
using CareerGuidance.Api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Threading;

namespace CareerGuidance.Api.Services
{
    public class EndPointHomePageService : IEndPointHomePageService
    {

        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IAllRoadmapsInsertedService _allRoadmapsInserted;

        public EndPointHomePageService(ApplicationDbContext applicationDbContext, IAllRoadmapsInsertedService allRoadmapsInserted)
        {
            _applicationDbContext = applicationDbContext;
            _allRoadmapsInserted = allRoadmapsInserted;
        }

        private bool IsValidString(string input)
        {
            string pattern = @"^(?![\W_\d])(?!(.*[\u0600-\u06FF]))(?!.*\s{2,})[A-Za-z][A-Za-z0-9\W_]*$";


            return Regex.IsMatch(input, pattern);
        }


        public async Task<Result<roadmapCategory>> AddRoadmapCategoryAsync([FromBody] roadmapCategoryDto roadmapCategory, CancellationToken cancellationToken = default)
        {

            if (string.IsNullOrEmpty(roadmapCategory.roadmapCategory))
                return Result.Failuer<roadmapCategory>(RoadmapError.CategoryNotEmpty);

             if (roadmapCategory.roadmapCategory.Length < 5)
                return Result.Failuer<roadmapCategory>(RoadmapError.CategoryNameLength);

            if (!IsValidString(roadmapCategory.roadmapCategory))
                return Result.Failuer<roadmapCategory>(RoadmapError.InvalidCategoryName);

            var find = await _applicationDbContext.RoadmapCategories.FirstOrDefaultAsync(R => R.Category == roadmapCategory.roadmapCategory, cancellationToken);

            if (find != null)
                return Result.Failuer<roadmapCategory>(RoadmapError.CategoryDublicated);

            var roadmapCat = new roadmapCategory
            {
                Category = roadmapCategory.roadmapCategory

            };

            _applicationDbContext.RoadmapCategories.Add(roadmapCat);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return Result.Success(roadmapCat);
        }

        //get All Gategories in Database
        public async Task<Result<IEnumerable<roadmapCategory>>> GetAllRoadmapCategoryAsync(CancellationToken cancellationToken = default)
        {
            var Categories = await _applicationDbContext.RoadmapCategories.ToListAsync();
            return Result.Success(Categories.Adapt<IEnumerable<roadmapCategory>>());
        }

        //Update Categories
        public async Task<Result> UpdateRoadmapCategoryAsync(CategoryIdDto categoryId, roadmapCategoryDto roadmapCategory, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(roadmapCategory.roadmapCategory))
                return Result.Failuer<roadmapCategory>(RoadmapError.CategoryNotEmpty);

            if (roadmapCategory.roadmapCategory.Length < 5 )
                return Result.Failuer<roadmapCategory>(RoadmapError.CategoryNameLength);

            var Check = await _applicationDbContext.RoadmapCategories.FirstOrDefaultAsync(x => x.Id == categoryId.Id, cancellationToken);

            if (Check == null)
                return Result.Failuer(RoadmapError.CategoryNotFound);

            var categoryNameExist = await _applicationDbContext.RoadmapCategories.AnyAsync(x => x.Category == roadmapCategory.roadmapCategory);
            if (categoryNameExist)
                return Result.Failuer(RoadmapError.CategoryDublicated);

            if (!IsValidString(roadmapCategory.roadmapCategory))
                return Result.Failuer<roadmapCategory>(RoadmapError.InvalidCategoryName);

            //اللي هيتبعت من اليوزر     هخزنه في القديم
            Check.Category = roadmapCategory.roadmapCategory;
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();

        }

        // Method to delete a roadmap category by its ID
        public async Task<Result> DeleteRoadmapCategoryAsync(CategoryIdDto categoryId, CancellationToken cancellationToken = default)
        {
            if (categoryId == null)
                return Result.Failuer(RoadmapError.CategoryNotEmpty);

            // Find the roadmap category by its ID
            var category = await _applicationDbContext.RoadmapCategories
                .FirstOrDefaultAsync(x => x.Id == categoryId.Id, cancellationToken);

            // If the category is not found, return false
            if (category == null)
            {
                return Result.Failuer(RoadmapError.CategoryNotFound);
            }

            // Find all ParsedRoadmaps linked to the category
            var parsedRoadmaps = await _applicationDbContext.ParsedRoadmaps
                .Where(x => x.CategoryId == categoryId.Id)
                .ToListAsync(cancellationToken);

            // Loop through each ParsedRoadmap and remove related Nodes and Edges
            foreach (var parsedRoadmap in parsedRoadmaps)
            {
                // Find and remove all Nodes linked to the ParsedRoadmap
                var nodes = await _applicationDbContext.Nodes
                    .Where(n => n.RoadmapId == parsedRoadmap.Id)
                    .ToListAsync(cancellationToken);

                foreach (var node in nodes)
                {
                    // Find and remove all Links linked to the Node
                    var links = await _applicationDbContext.Links
                        .Where(l => l.NodeId == node.Id)
                        .ToListAsync(cancellationToken);

                    _applicationDbContext.Links.RemoveRange(links);
                }

                _applicationDbContext.Nodes.RemoveRange(nodes);

                // Find and remove all Edges linked to the ParsedRoadmap
                var edges = await _applicationDbContext.Edges
                    .Where(e => e.RoadmapId == parsedRoadmap.Id)
                    .ToListAsync(cancellationToken);

                _applicationDbContext.Edges.RemoveRange(edges);
            }

            _applicationDbContext.ParsedRoadmaps.RemoveRange(parsedRoadmaps);

            // Remove all RoadmapData linked to the category
            var roadmapData = await _applicationDbContext.Roadmaps
                .Where(x => x.CategoryId == categoryId.Id)
                .ToListAsync(cancellationToken);

            _applicationDbContext.Roadmaps.RemoveRange(roadmapData);

            // Finally, remove the category itself
            _applicationDbContext.RoadmapCategories.Remove(category);

            // Save changes to the database, using cancellation token for async operation
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            // Return true if the deletion was successful
            return Result.Success();
        }

        public async Task<Result<GetCategoryResponse>> GetCategory(CategoryIdDto categoryId, CancellationToken cancellationToken = default)
        {
            var category = await _applicationDbContext.RoadmapCategories
              .FirstOrDefaultAsync(x => x.Id == categoryId.Id, cancellationToken);

            if (category == null)
                return Result.Failuer<GetCategoryResponse>(RoadmapError.CategoryNotFound);

            var result = category.Adapt<GetCategoryResponse>();

            return Result.Success(result);


        }





        /*

        // Adds a new introduction for a roadmap if the header does not already exist.
        // introductionInHome: The IntroductionInHomeDto object containing the introduction details.
        // cancellationToken: The cancellation token to monitor for cancellation requests.
        // Returns the newly added IntroductionInHome object.
        public async Task<InformationHomePageForRoadmap> AddInformationForRoadmapAsync(InformationHomePageForRoadmapDto informationHomePageForRoadmapDto, CancellationToken cancellationToken = default)
        {
            if (
                 !IsValidString(informationHomePageForRoadmapDto.description)
                 || !IsValidString(informationHomePageForRoadmapDto.header)
                     || !IsValidString(informationHomePageForRoadmapDto.status)
                )
            {
                throw new InvalidOperationException("Invalid Data");
            }


            var findDescription = await _applicationDbContext.InformationRoadmap.FirstOrDefaultAsync(i => i.description == informationHomePageForRoadmapDto.description, cancellationToken);

            if (findDescription != null)
            {
                throw new InvalidOperationException("description already exists.");
            }

            var findhHeader = await _applicationDbContext.InformationRoadmap.FirstOrDefaultAsync(i => i.header == informationHomePageForRoadmapDto.header, cancellationToken);

            if (findDescription != null)
            {
                throw new InvalidOperationException("header already exists.");
            }

            var findImage = await _applicationDbContext.InformationRoadmap.FirstOrDefaultAsync(i => i.Image == informationHomePageForRoadmapDto.Image, cancellationToken);

            if (findImage != null)
            {
                throw new InvalidOperationException("Image already exists.");
            }

            var Information = new InformationHomePageForRoadmap
            {
                description = informationHomePageForRoadmapDto.description,
                header = informationHomePageForRoadmapDto.header,
                Image = informationHomePageForRoadmapDto.Image,
                status = informationHomePageForRoadmapDto.status
            };

            _applicationDbContext.InformationRoadmap.Add(Information);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return Information;
        }

        public async Task<IEnumerable<InformationHomePageForRoadmap>> GetAllInformationForRoadmapAsync(CancellationToken cancellationToken = default )
        {
            return await _applicationDbContext.InformationRoadmap.ToListAsync();
        }


        // Updates an existing introduction for a roadmap by its ID.
        // introIdDto: The introIdDto object containing the ID of the introduction to update.
        // introductionInHome: The IntroductionInHomeDto object with updated introduction details.
        // cancellationToken: The cancellation token to monitor for cancellation requests.
        // Returns true if the update was successful; otherwise, false.
        public async Task<bool> UpdateInformationForRoadmapAsync(InformationHomePageForRoadmapId informationHomePageForRoadmapId, InformationHomePageForRoadmapDto informationHomePageForRoadmapDto, CancellationToken cancellationToken = default)
        {
            var find = await _applicationDbContext.InformationRoadmap.FindAsync(informationHomePageForRoadmapId.Id, cancellationToken);

            if (find == null)
            {
                return false;
            }

            if (
                  !IsValidString(informationHomePageForRoadmapDto.description) ||
                  !IsValidString(informationHomePageForRoadmapDto.header) ||
                  !IsValidString(informationHomePageForRoadmapDto.status)
                )
            {
                throw new InvalidOperationException("Invalid Data");
            }
            //new value not equal Current value
            if (find.description != informationHomePageForRoadmapDto.description)
            //if des for that id (input) equal des in database (no update was made)
            {
                ////check new value exist in data base or not
                var checkDescription = await _applicationDbContext.InformationRoadmap
                    .FirstOrDefaultAsync(x => x.description == informationHomePageForRoadmapDto.description, cancellationToken);
                if (checkDescription != null)
                {
                    throw new InvalidOperationException("description already exists.");
                }
            }

            if (find.Image != informationHomePageForRoadmapDto.Image)
            {
                var checkImage = await _applicationDbContext.InformationRoadmap
                    .FirstOrDefaultAsync(x => x.Image == informationHomePageForRoadmapDto.Image, cancellationToken);
                if (checkImage != null)
                {
                    throw new InvalidOperationException("Image already exists.");
                }
            }



            find.description = informationHomePageForRoadmapDto.description;
            find.status = informationHomePageForRoadmapDto.status;
            find.Image = informationHomePageForRoadmapDto.Image;
            find.header = informationHomePageForRoadmapDto.header;
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        // Deletes an introduction for a roadmap by its ID.
        // introIdDto: The introIdDto object containing the ID of the introduction to delete.
        // cancellationToken: The cancellation token to monitor for cancellation requests.
        // Returns true if the deletion was successful; otherwise, false.
        public async Task<bool> DeleteInformationForRoadmapAsync(InformationHomePageForRoadmapId informationHomePageForRoadmapId, CancellationToken cancellationToken = default)
        {
            var check = await _applicationDbContext.InformationRoadmap
                .FindAsync(informationHomePageForRoadmapId.Id, cancellationToken);

            if (check == null) { return false; }

            _applicationDbContext.InformationRoadmap.Remove(check);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<InformationHomePageForRoadmap> GetInformationForRoadmapAsync(InformationHomePageForRoadmapId informationHomePageForRoadmapId, CancellationToken cancellationToken = default)
        {
            if (informationHomePageForRoadmapId == null)
            {
                throw new InvalidOperationException("This filed Can not be empty");
            }

            var find = await _applicationDbContext.InformationRoadmap.FindAsync(informationHomePageForRoadmapId.Id, cancellationToken);
            if (find == null)
            {
                throw new InvalidOperationException("Not Found");
            }

            return find;
        }

        */






        public async Task<Result<IEnumerable<IntroductionHomePage>>> GetAllIntroductionHomePageAsync(CancellationToken cancellationToken = default)
        {
            var introductions = await _applicationDbContext.IntroductionHomePage
                .ToListAsync(cancellationToken);
            return Result.Success(introductions.Adapt<IEnumerable<IntroductionHomePage>>());
        }

        // Add Description in Home Page
        public async Task<Result<IntroductionHomePage>> AddIntroductionHomePageAsync(IntroductionHomePageDto introductionHomePageDto, CancellationToken cancellationToken = default)
        {
            if (!IsValidString(introductionHomePageDto.description))
                return Result.Failuer<IntroductionHomePage>(EndPointHomePageError.InvalidData);


            var check = await _applicationDbContext.IntroductionHomePage
                .FirstOrDefaultAsync(r => r.description == introductionHomePageDto.description, cancellationToken);

            if (check != null)
            {
                return Result.Failuer<IntroductionHomePage>(EndPointHomePageError.DescriptionDuplicated);
            }

            var result = new IntroductionHomePage
            {
                description = introductionHomePageDto.description
            };

            _applicationDbContext.IntroductionHomePage.Add(result);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return Result.Success(result);
        }

        // Delete Description in Home Page
        public async Task<Result> DeleteIntroductionHomePageAsync(IntroductionHomePageId introductionHomePageId, CancellationToken cancellationToken = default)
        {
            if (introductionHomePageId == null)
                return Result.Failuer(EndPointHomePageError.FieldRequired);

            var find = await _applicationDbContext.IntroductionHomePage
                .FirstOrDefaultAsync(i => i.Id == introductionHomePageId.Id, cancellationToken);

            if (find == null)
            {
                return Result.Failuer(EndPointHomePageError.NotFound);
            }

            _applicationDbContext.IntroductionHomePage.Remove(find);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        // Update Description in Home Page
        public async Task<Result> UpdateIntroductionHomePageAsync(IntroductionHomePageId introductionHomePageId, IntroductionHomePageDto introductionHomePageDto, CancellationToken cancellationToken = default)
        {
            if (introductionHomePageId == null)
            {
                return Result.Failuer(EndPointHomePageError.FieldRequired);
            }

            var checkFind = await _applicationDbContext.IntroductionHomePage
                .FirstOrDefaultAsync(i => i.Id == introductionHomePageId.Id, cancellationToken);

            if (checkFind == null)
            {
                return Result.Failuer(EndPointHomePageError.NotFound);
            }

            if (!IsValidString(introductionHomePageDto.description))
            {
                return Result.Failuer(EndPointHomePageError.InvalidData);
            }

            var checkDuplicated = await _applicationDbContext.IntroductionHomePage
                .AnyAsync(x => x.description == introductionHomePageDto.description && x.Id != introductionHomePageId.Id, cancellationToken);

            if (checkDuplicated)
                return Result.Failuer(EndPointHomePageError.DescriptionDuplicated);

            checkFind.description = introductionHomePageDto.description;
            _applicationDbContext.IntroductionHomePage.Update(checkFind);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        // Get Single Description in Home Page
        public async Task<Result<IntroductionHomePage>> GetIntroductionHomePageAsync(IntroductionHomePageId introductionHomePageId, CancellationToken cancellationToken = default)
        {
            if (introductionHomePageId == null)
            {
                return Result.Failuer<IntroductionHomePage>(EndPointHomePageError.FieldRequired);
            }

            var find = await _applicationDbContext.IntroductionHomePage
                .FirstOrDefaultAsync(i => i.Id == introductionHomePageId.Id, cancellationToken);

            if (find == null)
            {
                return Result.Failuer<IntroductionHomePage>(EndPointHomePageError.NotFound);
            }

            return Result.Success(find);
        }
    }
}

