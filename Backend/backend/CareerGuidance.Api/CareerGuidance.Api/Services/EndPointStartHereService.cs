
using CareerGuidance.Api.DTO;
using CareerGuidance.Api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using static System.Collections.Specialized.BitVector32;

namespace CareerGuidance.Api.Services
{
    public class EndPointStartHereService : IEndPointStartHereService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public EndPointStartHereService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        private bool IsValidString(string input)
        {
            string pattern = @"^(?![\W_\d])(?!(.*[\u0600-\u06FF]))(?!.*\s{2,})[A-Za-z][A-Za-z0-9\W_]*$";

            return Regex.IsMatch(input, pattern);
        }
      


        //public async Task<IntroductionStartHerePage> AddIntroductionStartHereAsync(IntroductionStartHerePageDto introductionInStartHerePageDto, CancellationToken cancellationToken = default)
        //{
        //    if (
        //       !IsValidString(introductionInStartHerePageDto.startHereIntroDes)
        //       || !IsValidString(introductionInStartHerePageDto.startHereIntroTitle))
        //    {
        //        throw new InvalidOperationException("Invalid Data");
        //    }


        //    var isDuplicate = await _applicationDbContext.IntroductionStartHere
        //    .AnyAsync(i =>

        //    i.startHereIntroTitle == introductionInStartHerePageDto.startHereIntroTitle,cancellationToken
        //    );

        //    if (isDuplicate)
        //    {
        //        throw new InvalidOperationException("Intro Title Is Dublicated");
        //    }

        //    var IntroStartHere = new IntroductionStartHerePage
        //    {

        //        startHereIntroDes = introductionInStartHerePageDto.startHereIntroDes,
        //        startHereIntroTitle = introductionInStartHerePageDto.startHereIntroTitle
        //    }; 

        //    _applicationDbContext.IntroductionStartHere.Add(IntroStartHere);
        //    await _applicationDbContext.SaveChangesAsync(cancellationToken);
        //    return IntroStartHere;
        //}


        //public async Task<bool> DeleteIntroductionInStartHereAsync(IntroductionStartHerePageId introductionInStartHerePageId, CancellationToken cancellationToken = default)
        //{
        //    if(introductionInStartHerePageId.Id == null) return false;

        //    var find = await _applicationDbContext.IntroductionStartHere.FindAsync(introductionInStartHerePageId.Id, cancellationToken);
        //    if (find == null)
        //    {
        //        throw new InvalidOperationException("Not Found.");
        //    }

        //    _applicationDbContext.IntroductionStartHere.Remove(find);
        //    await _applicationDbContext.SaveChangesAsync(cancellationToken);
        //    return true;
        //}


        //public async Task<IntroductionStartHerePage> GetIntroductionInStartHereAsync(IntroductionStartHerePageId introductionInStartHerePageId, CancellationToken cancellationToken = default)
        //{
        //    var find = await _applicationDbContext.IntroductionStartHere.FindAsync(introductionInStartHerePageId.Id, cancellationToken);
        //    if (find == null)
        //    {
        //        throw new InvalidOperationException("Not Found.");
        //    }
        //    return find;

        //}



        //public async Task<bool> UpdateIntroductionInStartHereAsync(IntroductionStartHerePageId introductionInStartHerePageId, IntroductionStartHerePageDto introductionInStartHerePageDto, CancellationToken cancellationToken = default)
        //{
        //    var find = await _applicationDbContext.IntroductionStartHere.FindAsync(introductionInStartHerePageId.Id, cancellationToken);
        //    if (find == null)
        //    {
        //        throw new InvalidOperationException("Not Found.");
        //    }

        //    else if (
        //           !IsValidString(introductionInStartHerePageDto.startHereIntroDes)
        //       || !IsValidString(introductionInStartHerePageDto.startHereIntroTitle)
        //       )
        //    {
        //        throw new InvalidOperationException("Invalid Data");
        //    }



        //    //var isDuplicateIntroDes = await _applicationDbContext.IntroInStartHerePage
        //    //    .AnyAsync(x => x.startHereIntroDes == introductionInStartHerePageDto.startHereIntroDes && x.Id != introductionInStartHerePageId.Id, cancellationToken);

        //    //if (isDuplicateIntroDes)
        //    //{
        //    //    throw new InvalidOperationException("startHereIntroDes already exists.");
        //    //}

        //    var isDuplicateIntroTitle = await _applicationDbContext.IntroductionStartHere
        //        .AnyAsync(x => x.startHereIntroTitle == introductionInStartHerePageDto.startHereIntroTitle && x.Id != introductionInStartHerePageId.Id, cancellationToken);

        //    if (isDuplicateIntroTitle)
        //    {
        //        throw new InvalidOperationException("startHereIntroTitle already exists.");
        //    }

        //    find.startHereIntroDes = introductionInStartHerePageDto.startHereIntroDes;
        //    find.startHereIntroTitle = introductionInStartHerePageDto.startHereIntroTitle;

        //    await _applicationDbContext.SaveChangesAsync(cancellationToken);
        //    return true;
        //}


        //public async Task<IEnumerable<IntroductionStartHerePage>> GetAllIntroductionInStartHereAsync(CancellationToken cancellationToken = default)
        //{
        //    return await _applicationDbContext.IntroductionStartHere.ToListAsync();
        //}










        //public async Task<ImportantStartHere> AddImportantStartHereAsync(ImportantStartHereDto  ImportantStartHereDto, CancellationToken cancellationToken = default)
        //{
        //    if (
        //          !IsValidString(ImportantStartHereDto.startHereImportanceDes)
        //       || !IsValidString(ImportantStartHereDto.startHereImportanceTitle))

        //    {
        //        throw new InvalidOperationException("Invalid Data");
        //    }


        //    var Duplicated = await _applicationDbContext.ImportantStartHere
        //    .AnyAsync(i =>
        //    i.startHereImportanceTitle== ImportantStartHereDto.startHereImportanceTitle,cancellationToken);

        //    if (Duplicated)
        //    {
        //        throw new InvalidOperationException("Important Title Is Dublicated");
        //    }

        //    var IntroStartHere = new ImportantStartHere
        //    {
        //        startHereImportanceTitle = ImportantStartHereDto.startHereImportanceTitle,
        //        startHereImportanceDes = ImportantStartHereDto.startHereImportanceDes,

        //    };

        //    _applicationDbContext.ImportantStartHere.Add(IntroStartHere);
        //    await _applicationDbContext.SaveChangesAsync(cancellationToken);
        //    return IntroStartHere;
        //}

        //public async Task<bool> UpdateImportantStartHereAsync(ImportantStartHereId  ImportantStartHereId, ImportantStartHereDto ImportantStartHereDto, CancellationToken cancellationToken = default)
        //{
        //    var find = await _applicationDbContext.ImportantStartHere.FindAsync(ImportantStartHereId.Id, cancellationToken);
        //    if (find == null)
        //    {
        //        throw new InvalidOperationException("Not Found.");
        //    }

        //    if (
        //         !IsValidString(ImportantStartHereDto.startHereImportanceDes)
        //        || !IsValidString(ImportantStartHereDto.startHereImportanceTitle)
        //       )
        //    {
        //        throw new InvalidOperationException("Invalid Data");
        //    }

        //    var isDuplicateTitle = await _applicationDbContext.ImportantStartHere
        //        .AnyAsync(x => x.startHereImportanceTitle == ImportantStartHereDto.startHereImportanceTitle && x.Id != ImportantStartHereId.Id, cancellationToken);

        //    if (isDuplicateTitle)
        //    {
        //        throw new InvalidOperationException("startHerImportanceTitle already exists.");
        //    }


        //    find.startHereImportanceTitle = ImportantStartHereDto.startHereImportanceTitle;
        //    find.startHereImportanceDes = ImportantStartHereDto.startHereImportanceDes;


        //    await _applicationDbContext.SaveChangesAsync(cancellationToken);
        //    return true;
        //}

        //public async Task<IEnumerable<ImportantStartHere>> GetAllImportantStartHereAsync(CancellationToken cancellationToken = default)
        //{
        //    return await _applicationDbContext.ImportantStartHere.ToListAsync();
        //}

        //public async Task<ImportantStartHere> GetImportantStartHereAsync(ImportantStartHereId ImportantStartHereId, CancellationToken cancellationToken = default)
        //{
        //    var find = await _applicationDbContext.ImportantStartHere.FindAsync(ImportantStartHereId.Id, cancellationToken);
        //    if (find == null)
        //    {
        //        throw new InvalidOperationException("Not Found.");
        //    }
        //    return find;

        //}

        //public async Task<bool> DeleteImportantStartHereAsync(ImportantStartHereId ImportantStartHereId, CancellationToken cancellationToken = default)
        //{
        //    if (ImportantStartHereId.Id == null) return false;

        //    var find = await _applicationDbContext.ImportantStartHere.FindAsync(ImportantStartHereId.Id, cancellationToken);
        //    if (find == null)
        //    {
        //        throw new InvalidOperationException("Not Found.");
        //    }

        //    _applicationDbContext.ImportantStartHere.Remove(find);
        //    await _applicationDbContext.SaveChangesAsync(cancellationToken);
        //    return true;
        //}


        public async Task<Result<IntroductionStartHerePage>> AddIntroductionStartHereAsync(IntroductionStartHerePageDto introductionInStartHerePageDto, CancellationToken cancellationToken = default)
        {
            if (!IsValidString(introductionInStartHerePageDto.startHereIntroDes) ||
                !IsValidString(introductionInStartHerePageDto.startHereIntroTitle))
            {
                return Result.Failuer<IntroductionStartHerePage>(EndPointStartHereError.InvalidIntroductionData);
            }


            var isDuplicate = await _applicationDbContext.IntroductionStartHere
                .AnyAsync(i => i.startHereIntroTitle == introductionInStartHerePageDto.startHereIntroTitle, cancellationToken);

            if (isDuplicate)
                return Result.Failuer<IntroductionStartHerePage>(EndPointStartHereError.DuplicateIntroductionTitle);

            var introStartHere = new IntroductionStartHerePage
            {
                startHereIntroDes = introductionInStartHerePageDto.startHereIntroDes,
                startHereIntroTitle = introductionInStartHerePageDto.startHereIntroTitle
            };

            _applicationDbContext.IntroductionStartHere.Add(introStartHere);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return Result.Success(introStartHere);
        }

        public async Task<Result> UpdateIntroductionInStartHereAsync(IntroductionStartHerePageId introductionInStartHerePageId, IntroductionStartHerePageDto introductionInStartHerePageDto, CancellationToken cancellationToken = default)
        {
            if (introductionInStartHerePageId.Id == null)
            {
                return Result.Failuer(EndPointStartHereError.FieldRequired);
            }

            var find = await _applicationDbContext.IntroductionStartHere
                .FirstOrDefaultAsync(i => i.Id == introductionInStartHerePageId.Id, cancellationToken);

            if (find == null)
                return Result.Failuer(EndPointStartHereError.NotFound);

            if (!IsValidString(introductionInStartHerePageDto.startHereIntroDes) ||
                !IsValidString(introductionInStartHerePageDto.startHereIntroTitle))
            {
                return Result.Failuer(EndPointStartHereError.InvalidIntroductionData);
            }

            var isDuplicateIntroTitle = await _applicationDbContext.IntroductionStartHere
                .AnyAsync(x => x.startHereIntroTitle == introductionInStartHerePageDto.startHereIntroTitle && x.Id != introductionInStartHerePageId.Id, cancellationToken);

            if (isDuplicateIntroTitle)
                return Result.Failuer(EndPointStartHereError.DuplicateIntroductionTitle);

            find.startHereIntroDes = introductionInStartHerePageDto.startHereIntroDes;
            find.startHereIntroTitle = introductionInStartHerePageDto.startHereIntroTitle;

            _applicationDbContext.IntroductionStartHere.Update(find);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> DeleteIntroductionInStartHereAsync(IntroductionStartHerePageId introductionInStartHerePageId, CancellationToken cancellationToken = default)
        {
            if (introductionInStartHerePageId.Id == null)
                return Result.Failuer(EndPointStartHereError.FieldRequired);

            var find = await _applicationDbContext.IntroductionStartHere
                .FirstOrDefaultAsync(i => i.Id == introductionInStartHerePageId.Id, cancellationToken);

            if (find == null)
            {
                return Result.Failuer(EndPointStartHereError.NotFound);
            }

            _applicationDbContext.IntroductionStartHere.Remove(find);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result<IntroductionStartHerePage>> GetIntroductionInStartHereAsync(IntroductionStartHerePageId introductionInStartHerePageId, CancellationToken cancellationToken = default)
        {
            if (introductionInStartHerePageId.Id == null)
                return Result.Failuer<IntroductionStartHerePage>(EndPointStartHereError.FieldRequired);

            var find = await _applicationDbContext.IntroductionStartHere
                .FirstOrDefaultAsync(i => i.Id == introductionInStartHerePageId.Id, cancellationToken);

            if (find == null)
                return Result.Failuer<IntroductionStartHerePage>(EndPointStartHereError.NotFound);

            return Result.Success(find);
        }

        public async Task<Result<IEnumerable<IntroductionStartHerePage>>> GetAllIntroductionInStartHereAsync(CancellationToken cancellationToken = default)
        {
            var IntroductionStartHere = await _applicationDbContext.IntroductionStartHere.ToListAsync(cancellationToken);
            return Result.Success(IntroductionStartHere.Adapt<IEnumerable<IntroductionStartHerePage>>());
        }

        // ImportantStartHere Methods
        public async Task<Result<ImportantStartHere>> AddImportantStartHereAsync(ImportantStartHereDto importantStartHereDto, CancellationToken cancellationToken = default)
        {
            if (!IsValidString(importantStartHereDto.startHereImportanceDes) ||
                !IsValidString(importantStartHereDto.startHereImportanceTitle))
            {
                return Result.Failuer<ImportantStartHere>(EndPointStartHereError.InvalidImportantData);
            }

            var duplicated = await _applicationDbContext.ImportantStartHere
                .AnyAsync(i => i.startHereImportanceTitle == importantStartHereDto.startHereImportanceTitle, cancellationToken);

            if (duplicated)
                return Result.Failuer<ImportantStartHere>(EndPointStartHereError.DuplicateImportantTitle);

            var introStartHere = new ImportantStartHere
            {
                startHereImportanceTitle = importantStartHereDto.startHereImportanceTitle,
                startHereImportanceDes = importantStartHereDto.startHereImportanceDes
            };

            _applicationDbContext.ImportantStartHere.Add(introStartHere);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return Result.Success(introStartHere);
        }

        public async Task<Result> UpdateImportantStartHereAsync(ImportantStartHereId importantStartHereId, ImportantStartHereDto importantStartHereDto, CancellationToken cancellationToken = default)
        {
            if (importantStartHereId.Id == null)
                return Result.Failuer(EndPointStartHereError.FieldRequired);

            var find = await _applicationDbContext.ImportantStartHere
                .FirstOrDefaultAsync(i => i.Id == importantStartHereId.Id, cancellationToken);

            if (find == null)
                return Result.Failuer(EndPointStartHereError.NotFound);

            if (!IsValidString(importantStartHereDto.startHereImportanceDes) ||
                !IsValidString(importantStartHereDto.startHereImportanceTitle))
            {
                return Result.Failuer(EndPointStartHereError.InvalidImportantData);
            }

            var isDuplicateTitle = await _applicationDbContext.ImportantStartHere
                .AnyAsync(x => x.startHereImportanceTitle == importantStartHereDto.startHereImportanceTitle && x.Id != importantStartHereId.Id, cancellationToken);

            if (isDuplicateTitle)
            {
                return Result.Failuer(EndPointStartHereError.DuplicateImportantTitle);
            }

            find.startHereImportanceTitle = importantStartHereDto.startHereImportanceTitle;
            find.startHereImportanceDes = importantStartHereDto.startHereImportanceDes;

            _applicationDbContext.ImportantStartHere.Update(find);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> DeleteImportantStartHereAsync(ImportantStartHereId importantStartHereId, CancellationToken cancellationToken = default)
        {
            if (importantStartHereId == null || importantStartHereId.Id == null)
            {
                return Result.Failuer(EndPointStartHereError.FieldRequired);
            }

            var find = await _applicationDbContext.ImportantStartHere
                .FirstOrDefaultAsync(i => i.Id == importantStartHereId.Id, cancellationToken);

            if (find == null)
            {
                return Result.Failuer(EndPointStartHereError.NotFound);
            }

            _applicationDbContext.ImportantStartHere.Remove(find);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result<ImportantStartHere>> GetImportantStartHereAsync(ImportantStartHereId importantStartHereId, CancellationToken cancellationToken = default)
        {
            if (importantStartHereId == null || importantStartHereId.Id == null)
                return Result.Failuer<ImportantStartHere>(EndPointStartHereError.FieldRequired);

            var find = await _applicationDbContext.ImportantStartHere
                .FirstOrDefaultAsync(i => i.Id == importantStartHereId.Id, cancellationToken);

            if (find == null)
            {
                return Result.Failuer<ImportantStartHere>(EndPointStartHereError.NotFound);
            }

            return Result.Success(find);
        }

        public async Task<Result<IEnumerable<ImportantStartHere>>> GetAllImportantStartHereAsync(CancellationToken cancellationToken = default)
        {
            var ImportantItemStartHere = await _applicationDbContext.ImportantStartHere.ToListAsync(cancellationToken);
            return Result.Success(ImportantItemStartHere.Adapt<IEnumerable<ImportantStartHere>>());
        }







        public async Task<Result<NewCarouselSection>> AddCarouselSectionAsync(NewCarouselSectionDto newCarouselSectionDto, CancellationToken cancellationToken = default)
        {
            if (newCarouselSectionDto.newCarouselSection.Length < 3)
                return Result.Failuer<NewCarouselSection>(EndPointStartHereError.CarouselSectionLength);

            if (!IsValidString(newCarouselSectionDto.newCarouselSection))
                return Result.Failuer<NewCarouselSection>(EndPointStartHereError.InvalidCarouselSectionData);


            var Duplicated = await _applicationDbContext.NewCarouselSection
                .AnyAsync(s => s.newCarouselSection == newCarouselSectionDto.newCarouselSection, cancellationToken);

            if (Duplicated)
                return Result.Failuer<NewCarouselSection>(EndPointStartHereError.DuplicateCarouselSection);


            var sectionTypeRoadmap = new NewCarouselSection
            {
                newCarouselSection = newCarouselSectionDto.newCarouselSection
            };

            _applicationDbContext.NewCarouselSection.Add(sectionTypeRoadmap);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return Result.Success(sectionTypeRoadmap);
        }

        public async Task<Result> UpdateCarouselSectionAsync(NewCarouselSectionId newCarouselSectionId, NewCarouselSectionDto newCarouselSectionDto, CancellationToken cancellationToken = default)
        {

            if (newCarouselSectionDto.newCarouselSection.Length < 3)
                return Result.Failuer<NewCarouselSection>(EndPointStartHereError.CarouselSectionLength);

            if (!IsValidString(newCarouselSectionDto.newCarouselSection))
                return Result.Failuer(EndPointStartHereError.InvalidCarouselSectionData);


            var CheckFind = await _applicationDbContext.NewCarouselSection
                .FirstOrDefaultAsync(s => s.Id == newCarouselSectionId.Id, cancellationToken);

            if (CheckFind == null)
                return Result.Failuer(EndPointStartHereError.NotFound);


            var Duplicated = await _applicationDbContext.NewCarouselSection

                .AnyAsync(s => s.newCarouselSection == newCarouselSectionDto.newCarouselSection, cancellationToken);

            if (Duplicated)
                return Result.Failuer(EndPointStartHereError.DuplicateCarouselSection);

            CheckFind.newCarouselSection = newCarouselSectionDto.newCarouselSection;

            _applicationDbContext.NewCarouselSection.Update(CheckFind);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result> DeleteCarouselSectionAsync(NewCarouselSectionId newCarouselSectionId, CancellationToken cancellationToken = default)
        {
            if (newCarouselSectionId == null)
                return Result.Failuer(EndPointStartHereError.FieldRequired);

            var sectionTypeRoadmap = await _applicationDbContext.NewCarouselSection
                                        .FirstOrDefaultAsync(s => s.Id == newCarouselSectionId.Id, cancellationToken);

            if (sectionTypeRoadmap == null)
                return Result.Failuer(EndPointStartHereError.NotFound);

            _applicationDbContext.DetailsCarouselSection.RemoveRange(sectionTypeRoadmap.DetailsCarouselSection);

            _applicationDbContext.NewCarouselSection.Remove(sectionTypeRoadmap);

            await _applicationDbContext.SaveChangesAsync(cancellationToken);


            return Result.Success();
        }

        public async Task<Result<IEnumerable<NewCarouselSection>>> GetAllCarouselSectionAsync(CancellationToken cancellationToken = default)
        {
            var CarouselSections = await _applicationDbContext.NewCarouselSection.ToListAsync(cancellationToken);
            return Result.Success(CarouselSections.Adapt<IEnumerable<NewCarouselSection>>());
        }

        public async Task<Result<NewCarouselSection>> GetCarouselSectionAsync(NewCarouselSectionId newCarouselSectionId, CancellationToken cancellationToken = default)
        {
            if (newCarouselSectionId == null)
                return Result.Failuer<NewCarouselSection>(EndPointStartHereError.FieldRequired);

            var CheckCarouselSection = await _applicationDbContext.NewCarouselSection
                                             .FirstOrDefaultAsync(s => s.Id == newCarouselSectionId.Id, cancellationToken);

            if (CheckCarouselSection == null)
                return Result.Failuer<NewCarouselSection>(EndPointStartHereError.NotFound);


            return Result.Success(CheckCarouselSection);
        }







        public async Task<Result<DetailsCarouselSection>> AddDetailsCarouselSectionAsync(DetailsCarouselSectionDto detailsCarouselSectionDto, CancellationToken cancellationToken = default)
        {
            if (detailsCarouselSectionDto.carouselSection.Length < 3)
                return Result.Failuer<DetailsCarouselSection>(EndPointStartHereError.CarouselSectionLength);

            if (detailsCarouselSectionDto.carouselDes.Length < 3 )
                return Result.Failuer<DetailsCarouselSection>(EndPointStartHereError.CarouselDescriptionLength);

          
            if (detailsCarouselSectionDto.carouselTitle.Length < 3)
                return Result.Failuer<DetailsCarouselSection>(EndPointStartHereError.CarouselTitleLength);


            if (!Uri.TryCreate(detailsCarouselSectionDto.carouselUrl, UriKind.Absolute, out var uriResult)
              || (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
            {
                return Result.Failuer<DetailsCarouselSection>(EndPointStartHereError.InvalidCarouselUrl);
            }

            if (!IsValidString(detailsCarouselSectionDto.carouselDes))
                return Result.Failuer<DetailsCarouselSection>(EndPointStartHereError.InvalidCarouselDescription);


            if (detailsCarouselSectionDto.carouselState != null)
            {

                if (!IsValidString(detailsCarouselSectionDto.carouselState))
                    return Result.Failuer<DetailsCarouselSection>(EndPointStartHereError.InvalidCarouselState);
                
                if (detailsCarouselSectionDto.carouselState.Length < 3)
                    return Result.Failuer<DetailsCarouselSection>(EndPointStartHereError.CarouselStateLength);
            }

            if (!IsValidString(detailsCarouselSectionDto.carouselTitle))
                return Result.Failuer<DetailsCarouselSection>(EndPointStartHereError.InvalidCarouselTitle);


            if (!IsValidString(detailsCarouselSectionDto.carouselImg))
                return Result.Failuer<DetailsCarouselSection>(EndPointStartHereError.InvalidCarouselImg);


            var FindNewCarouselSection = await _applicationDbContext.NewCarouselSection
                .FirstOrDefaultAsync(s => s.newCarouselSection == detailsCarouselSectionDto.carouselSection, cancellationToken);

            if (FindNewCarouselSection == null)
                return Result.Failuer<DetailsCarouselSection>(EndPointStartHereError.SectionNotFound);


            ///var Dublicated = await _applicationDbContext.DetailsForCarousel.AnyAsync(d =>
            ///                       d.carouselState == detailsForCarouselDto.carouselState ||
            ///                       d.carouselTitle == detailsForCarouselDto.carouselTitle ||
            ///                       d.carouselDes == detailsForCarouselDto.carouselDes ||
            ///                       d.carouselImg == detailsForCarouselDto.carouselImg, cancellationToken);
            ///
            ///if (Dublicated)
            ///{
            ///    throw new InvalidOperationException("Duplicated Data");
            ///}

            var detailsCarouselSection = new DetailsCarouselSection
            {
                carouselSection = detailsCarouselSectionDto.carouselSection,
                carouselState = detailsCarouselSectionDto.carouselState,
                carouselTitle = detailsCarouselSectionDto.carouselTitle,
                carouselDes = detailsCarouselSectionDto.carouselDes,
                carouselImg = detailsCarouselSectionDto.carouselImg,
                carouselUrl = detailsCarouselSectionDto.carouselUrl,
                CarouselSectionId = FindNewCarouselSection.Id
            };


            _applicationDbContext.DetailsCarouselSection.Add(detailsCarouselSection);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return Result.Success(detailsCarouselSection);
        }

        public async Task<Result> DeleteDetailsCarouselSectionAsync(DetailsCarouselSectionId detailsCarouselSectionId, CancellationToken cancellationToken = default)
        {

            if (detailsCarouselSectionId.Id == null)
                return Result.Failuer(EndPointStartHereError.FieldRequired);

            // Find  Carousel
            var detailsCarouselSection = await _applicationDbContext.DetailsCarouselSection
                .FirstOrDefaultAsync(d => d.Id == detailsCarouselSectionId.Id, cancellationToken);

            if (detailsCarouselSection == null)
                return Result.Failuer(EndPointStartHereError.NotFound);

            _applicationDbContext.DetailsCarouselSection.Remove(detailsCarouselSection);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result> UpdateDetailsCarouselSectionAsync(DetailsCarouselSectionId detailsCarouselSectionId, DetailsCarouselSectionDto detailsForCarouselDto, CancellationToken cancellationToken = default)
        {
            if (detailsForCarouselDto.carouselSection.Length < 3)
                return Result.Failuer<DetailsCarouselSection>(EndPointStartHereError.CarouselSectionLength);

            if (detailsForCarouselDto.carouselDes.Length < 3)
                return Result.Failuer<DetailsCarouselSection>(EndPointStartHereError.CarouselDescriptionLength);

           
            if (detailsForCarouselDto.carouselTitle.Length < 3)
                return Result.Failuer<DetailsCarouselSection>(EndPointStartHereError.CarouselTitleLength);


            if (!Uri.TryCreate(detailsForCarouselDto.carouselUrl, UriKind.Absolute, out var uriResult)
              || (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
            {
                return Result.Failuer<DetailsCarouselSection>(EndPointStartHereError.InvalidCarouselUrl);
            }


            if (!IsValidString(detailsForCarouselDto.carouselDes))
                return Result.Failuer<DetailsCarouselSection>(EndPointStartHereError.InvalidCarouselDescription);

            if (detailsForCarouselDto.carouselState != null)
            {

                if (!IsValidString(detailsForCarouselDto.carouselState))
                    return Result.Failuer<DetailsCarouselSection>(EndPointStartHereError.InvalidCarouselState);

                if (detailsForCarouselDto.carouselState.Length < 3)
                    return Result.Failuer<DetailsCarouselSection>(EndPointStartHereError.CarouselStateLength);
            }

            if (!IsValidString(detailsForCarouselDto.carouselTitle))
                return Result.Failuer<DetailsCarouselSection>(EndPointStartHereError.InvalidCarouselTitle);


            if (!IsValidString(detailsForCarouselDto.carouselImg))
                return Result.Failuer<DetailsCarouselSection>(EndPointStartHereError.InvalidCarouselImg);


            // Find the existing record
            var details = await _applicationDbContext.DetailsCarouselSection
                .FirstOrDefaultAsync(d => d.Id == detailsCarouselSectionId.Id, cancellationToken);

            if (details == null)
                return Result.Failuer(EndPointStartHereError.NotFound);


            // Ensure the CarouselSection exists
            var section = await _applicationDbContext.NewCarouselSection
                .FirstOrDefaultAsync(s => s.newCarouselSection == detailsForCarouselDto.carouselSection, cancellationToken);

            if (section == null)
                return Result.Failuer(EndPointStartHereError.SectionNotFound);

            //var Duplicated = await _applicationDbContext.DetailsCarouselSection
            //    .AnyAsync(d => d.carouselTitle == detailsForCarouselDto.carouselTitle, cancellationToken);

            //if (Duplicated)
            //{
            //    return Result.Failuer(EndPointStartHereError.DuplicateCarouselTitle);
            //}

            details.carouselSection = detailsForCarouselDto.carouselSection;
            details.carouselState = detailsForCarouselDto.carouselState;
            details.carouselTitle = detailsForCarouselDto.carouselTitle;
            details.carouselDes = detailsForCarouselDto.carouselDes;
            details.carouselImg = detailsForCarouselDto.carouselImg;
            details.carouselUrl = detailsForCarouselDto.carouselUrl;
            details.CarouselSectionId = section.Id;

            _applicationDbContext.DetailsCarouselSection.Update(details);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result<DetailsCarouselSection>> GetDetailsCarouselSectionAsync(DetailsCarouselSectionId detailsCarouselSectionId, CancellationToken cancellationToken = default)
        {
            if (detailsCarouselSectionId.Id == null)
                return Result.Failuer<DetailsCarouselSection>(EndPointStartHereError.FieldRequired);

            var detailsCarouselSection = await _applicationDbContext.DetailsCarouselSection
                .FirstOrDefaultAsync(d => d.Id == detailsCarouselSectionId.Id, cancellationToken);

            if (detailsCarouselSection == null)
                return Result.Failuer<DetailsCarouselSection>(EndPointStartHereError.NotFound);

            return Result.Success(detailsCarouselSection);
        }
        public async Task<Result<IEnumerable<DetailsCarouselSection>>> GetAllDetailsCarouselSectionAsync(CancellationToken cancellationToken = default)
        {
            var DetailsCarouselSection = await _applicationDbContext.DetailsCarouselSection.ToListAsync(cancellationToken);
            return Result.Success(DetailsCarouselSection.Adapt<IEnumerable<DetailsCarouselSection>>());
        }



    }
}
