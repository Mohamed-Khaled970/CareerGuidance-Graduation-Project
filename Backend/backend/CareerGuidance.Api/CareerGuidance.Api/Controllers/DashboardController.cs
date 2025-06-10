/*
 * File Name: DashboardController.cs
 * Author Information: Abdelrahman Rezk , Abdelrahman Maged
 * Date of Creation: 2024-10-13
 * Version Information: v1.0
 * Dependencies: CareerGuidance.Api.Services
 * Contributors: Abdelrahman Rezk
 * Last Modified Date: 2024-10-27
 *
 * Description:
 *      Controller for managing dashboard-related functionalities,
 *      including user and question operations, 
 *      as well as handling roadmap data*/

using CareerGuidance.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using CareerGuidance.Api.DTO;
using CareerGuidance.Api.Models;
using CareerGuidance.Api.Abstractions.Const;
namespace CareerGuidance.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService dashboardService;
        private readonly IAllRoadmapsInsertedService _roadmapService;
        private readonly IEndPointHomePageService _endPointInHomePage;
        private readonly IEndPointStartHereService _endPointInStartHere;

        public DashboardController(IDashboardService dashboardService, IAllRoadmapsInsertedService allroadmap, IEndPointHomePageService endPointInHomePage,IEndPointStartHereService endPointInStartHere)
        {
            this.dashboardService = dashboardService;
            _roadmapService = allroadmap;   
            _endPointInHomePage = endPointInHomePage;
            _endPointInStartHere = endPointInStartHere;
        }

        // Retrieves all users
        [HttpGet("GetAllUsers")]
        [Authorize(Roles = DefaultRole.Admin)]
        public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken = default)
        {
            var result = await dashboardService.GetAllUsers(cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("GetuserById/{id:guid}")]
        [Authorize(Roles = DefaultRole.Admin)]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var results = await dashboardService.GetUserById(id, cancellationToken);
            return results.IsSuccess ? Ok(results.Value) : results.ToProblem();
        }


        // Adds a new user
        [HttpPost("AddUser")]
        [Authorize(Roles = DefaultRole.Admin)]
        public async Task<IActionResult> AddUser(AddNewUserRequest request, CancellationToken cancellationToken = default)
        {
            var result = await dashboardService.AddNewUserAsync(request, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }

        [HttpPut("UpdateAdminProfile")]
        [Authorize(Roles = DefaultRole.Admin)]
        public async Task<IActionResult> UpdateAdminProfile (UpdateAdminProfileRequest request, CancellationToken cancellationToken = default)
        {
            var result = await dashboardService.UpdateAdminProfile(request, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }

        // Deletes a specific question by ID
        [HttpDelete("DeleteUser/{id}")]
        [Authorize(Roles = DefaultRole.Admin)]
        public async Task<IActionResult> DeleteUser([FromRoute] string id, CancellationToken cancellationToken)
        {
            var result = await dashboardService.DeleteUserAsync(id, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }



        // Adds a new question
        [HttpPost("AddQuestion")]
        [Authorize(Roles = DefaultRole.Admin)]
        public async Task<IActionResult> AddQuestion(QuestionRequest request, CancellationToken cancellationToken = default)
        {
            var result = await dashboardService.AddQuestion(request, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }

        // Retrieves all questions
        [HttpGet("GetAllQuestions")]
        public async Task<IActionResult> GetAllQuestions(CancellationToken cancellationToken = default)
        {
            var result = await dashboardService.GetAllQuestions(cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }



        // Updates a specific question by ID

        [HttpPut("UpdateQuestion/{id}")]
        [Authorize(Roles = DefaultRole.Admin)]
        public async Task<IActionResult> UpdateQuestion([FromRoute] int id, [FromBody] QuestionRequest request, CancellationToken cancellationToken)
        {
            var result = await dashboardService.UpdateQuestion(id, request, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        // Deletes a specific question by ID
        [HttpDelete("DeleteQuestion/{id}")]
        [Authorize(Roles = DefaultRole.Admin)]
        public async Task<IActionResult> DeleteQuestion([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await dashboardService.DeleteQuestion(id, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        // Adds roadmap data to the database
        [HttpPost("AddRoadmapData")]
        [Authorize(Roles = DefaultRole.Admin + "," + DefaultRole.Instructor)]
        public async Task<IActionResult> AddDataForRoadmap([FromBody] allRoadmapInsertedDto roadmapDto, CancellationToken cancellationToken = default)
        {

            var result = await _roadmapService.AddFilteredRoadmapAsync(roadmapDto, cancellationToken);

            return result.IsSuccess ? Ok() : result.ToProblem();
        }


        // Retrieves all roadmaps from the database
        [HttpGet("GetALlRoadmapsInDatabase")]
        public async Task<IActionResult> GetAllRoadmapsAsync(CancellationToken cancellationToken = default)
        {
            var result = await _roadmapService.GetAllAsync( cancellationToken);
            return Ok(result);
        }

        // Retrieves a specific roadmap by ID
        [HttpGet("GetById/{id:guid}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var roadmap = await _roadmapService.GetByIdAsync(id, cancellationToken);
            return roadmap is null ? NotFound() : Ok(roadmap);
        }


        // Endpoint that checks if a roadmap exists by name
        [HttpPost("CheckRoadmapInformation")]
        [Authorize(Roles = DefaultRole.Admin + "," + DefaultRole.Instructor)]
        public async Task<IActionResult> CheckRoadmapInformationAsync([FromBody] CreateRoadmapRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _roadmapService.CheckRoadmapInformationAsync(request, cancellationToken);

            return result.IsSuccess ? Ok()
                : result.ToProblem();
        }



        //EndPoint For Home page


        // Adds Category to the database
        [HttpPost("AddRoadmapCategory")]
        [Authorize(Roles = DefaultRole.Admin + "," + DefaultRole.Instructor)]
        public async Task<IActionResult> AddCategoryAsync([FromBody] roadmapCategoryDto roadmapCategory, CancellationToken cancellationToken = default)
        {

            // Validate request data
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }

            var result = await _endPointInHomePage.AddRoadmapCategoryAsync(roadmapCategory, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }

        // Retrieves all Catigories from the database
        [HttpGet("GetAllCategoryInDatabase")]
        public async Task<IActionResult> GetAllCategoryAsync(CancellationToken cancellationToken = default)
        {
            var result = await _endPointInHomePage.GetAllRoadmapCategoryAsync(cancellationToken);
            return result.IsSuccess? Ok(result.Value) : result.ToProblem();
        }


        // Updates a specific Category by ID
        [HttpPut("UpdateRoadmapCategory/{Id}")]//send id for function UpdateRoadmapCategoryAsync
        [Authorize(Roles = DefaultRole.Admin + "," + DefaultRole.Instructor)]
        public async Task<IActionResult> UpdateCategoryAsync([FromRoute] CategoryIdDto categoryId, roadmapCategoryDto roadmap_, CancellationToken cancellationToken = default)
        {
            var categoryIdDto = new CategoryIdDto { Id = categoryId.Id };
            var result = await _endPointInHomePage.UpdateRoadmapCategoryAsync(categoryIdDto, roadmap_, cancellationToken);
            return result.IsSuccess ? Ok() :
                result.ToProblem();
        }

        [HttpGet("GetCategory/{Id}")]//send id for function UpdateRoadmapCategoryAsync
        public async Task<IActionResult> GetCategory([FromRoute] CategoryIdDto categoryId, CancellationToken cancellationToken = default)
        {
            var result = await _endPointInHomePage.GetCategory(categoryId, cancellationToken);
            return result.IsSuccess ? Ok(result.Value)
                : result.ToProblem();
        }

        // Deletes a specific Category by ID
        [HttpDelete("DeleteRoadmapCategory/{Id}")]
        [Authorize(Roles = DefaultRole.Admin + "," + DefaultRole.Instructor)]
        public async Task<IActionResult> DeleteCategoryAsync([FromRoute] CategoryIdDto categoryId, CancellationToken cancellationToken = default)
        {
            var categoryIdDto = new CategoryIdDto { Id = categoryId.Id };
            var result = await _endPointInHomePage.DeleteRoadmapCategoryAsync(categoryIdDto, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }


        // Updates a specific roadmap by ID
        [HttpPut("Update/{id:guid}")]
        [Authorize(Roles = DefaultRole.Admin + "," + DefaultRole.Instructor)]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, allRoadmapInsertedDto roadmap_, CancellationToken cancellationToken = default)
        {
            var result = await _roadmapService.UpdateAsync(id, roadmap_, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem();

        }

        // Deletes a specific roadmap by ID
        [HttpDelete("Delete/{id:guid}")]
        [Authorize(Roles = DefaultRole.Admin + "," + DefaultRole.Instructor)]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _roadmapService.DeleteAsync(id, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();

        }

        [HttpPost("UpdateRoadmapInformation")]
        [Authorize(Roles = DefaultRole.Admin + "," + DefaultRole.Instructor)]
        public async Task<IActionResult> UpdateRoadmapInformationAsync([FromBody] UpdateRoadmapRequest request, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }
            var result = await _roadmapService.UpdateRoadmapInformationAsync(request, cancellationToken);

            return result.IsSuccess ? Ok() : result.ToProblem();
        }





        /*
        //[HttpPost("AddRoadmapinformation")]
        //public async Task<IActionResult> AddRoadmapinformation([FromBody] InformationHomePageForRoadmapDto  informationHomePageForRoadmapDto, CancellationToken cancellationToken = default)
        //{

        //    // Validate request data
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest("Invalid data.");
        //    }

        //    //try
        //    //{
        //    var allcategory = await _endPointInHomePage.AddInformationForRoadmapAsync(informationHomePageForRoadmapDto, cancellationToken);
               
        //    return Ok();
        //    //}
        //    //catch (InvalidOperationException ex)
        //    //{
        //    //    return Conflict(ex.Message); // Conflict due to existing data
        //    //}
        //    //catch (ArgumentException ex)
        //    //{
        //    //    return BadRequest(ex.Message);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    return StatusCode(500, $"Internal server error: {ex.Message}");
        //    //}
        //}


        //// Retrieves all Information Roadmap from the database
        //[HttpGet("GetAllInformationInDatabase")]
        //public async Task<IActionResult> GetAllInformationInDatabase(CancellationToken cancellationToken = default)
        //{
        //    var result = await _endPointInHomePage.GetAllInformationForRoadmapAsync(cancellationToken);
        //    return Ok(result);
        //}


        //[HttpPut("UpdateInformationRoadmap/{Id}")]//send id for function UpdateRoadmapCategoryAsync
        //public async Task<IActionResult> UpdateInformationRoadmap([FromRoute] InformationHomePageForRoadmapId  informationHomePageForRoadmapId, InformationHomePageForRoadmapDto introductionInHomeForRoadmapDto, CancellationToken cancellationToken = default)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest("Invalid Data");
        //    }
        //    var informationIdDto = new InformationHomePageForRoadmapId { Id = informationHomePageForRoadmapId.Id };
        //    var result = await _endPointInHomePage.UpdateInformationForRoadmapAsync(informationIdDto, introductionInHomeForRoadmapDto, cancellationToken);
        //    return result ? Ok() : NotFound();
        //}


        //[HttpDelete("DeleteInformationRoadmap/{Id}")]
        //public async Task<IActionResult> DeleteInformationRoadmapAsync([FromRoute] InformationHomePageForRoadmapId informationHomePageForRoadmapId, CancellationToken cancellationToken = default)
        //{
        //    var informationIdDto = new InformationHomePageForRoadmapId { Id = informationHomePageForRoadmapId.Id };
        //    var result = await _endPointInHomePage.DeleteInformationForRoadmapAsync(informationIdDto, cancellationToken);
        //    return result ? Ok() : NotFound();
        //}

        //[HttpGet("GetInformationForRoadmap/{Id}")]
        //public async Task<IActionResult> GetInformationForRoadmap([FromRoute] InformationHomePageForRoadmapId  informationHomePageForRoadmapId, CancellationToken cancellationToken = default)
        //{
        //    if (informationHomePageForRoadmapId.Id == null) 
        //    {
        //        return BadRequest("This Field Can not be empty.");
        //    }
        //    var result = await _endPointInHomePage.GetInformationForRoadmapAsync(informationHomePageForRoadmapId, cancellationToken);
        //    if (result == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(result);

        //}



        */

        [HttpPost("AddIntroductionHomePage")]
        [Authorize(Roles = DefaultRole.Admin)]
        public async Task<IActionResult> AddIntroductionHomePage([FromBody] IntroductionHomePageDto introductionHomePageDto, CancellationToken cancellationToken    = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }
            var result = await _endPointInHomePage.AddIntroductionHomePageAsync(introductionHomePageDto, cancellationToken);

            if (result == null)
            {
                return Conflict( "Invalid Data");
            }

            return result.IsSuccess? Ok() : result.ToProblem() ;
        }


        [HttpPut("UpdateIntroductionHomePage/{Id}")]
        [Authorize(Roles = DefaultRole.Admin)]
        public async Task<IActionResult> UpdateIntroductionHomePage([FromRoute] IntroductionHomePageId introductionHomePageId, IntroductionHomePageDto introductionHomePageDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }
            var introductionIdDto = new IntroductionHomePageId { Id = introductionHomePageId.Id };
            var result = await _endPointInHomePage.UpdateIntroductionHomePageAsync(introductionIdDto, introductionHomePageDto, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }

        [HttpGet("GetAllIntroductionHomePage")]
        public async Task<IActionResult> GetAllIntroductionHomePage(CancellationToken cancellationToken = default)
        {
            var result = await _endPointInHomePage.GetAllIntroductionHomePageAsync(cancellationToken);
            return result.IsSuccess? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("GetIntroductionHomePage/{Id}")]
        public async Task<IActionResult> GetIntroductionHomePage([FromRoute] IntroductionHomePageId introductionHomePageId, CancellationToken cancellationToken = default)
        {
           
            if (introductionHomePageId == null )
            {
                return BadRequest("Invalid Data");
            }
            var result = await _endPointInHomePage.GetIntroductionHomePageAsync(introductionHomePageId, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

        }


        [HttpDelete("DeleteIntroductionHomePage/{Id}")]
        [Authorize(Roles = DefaultRole.Admin)]
        public async Task<IActionResult> DeleteIntroductionHomePage([FromRoute] IntroductionHomePageId introductionHomePageId, CancellationToken cancellationToken = default)
        {

            if (introductionHomePageId == null || !ModelState.IsValid )
            {
                return BadRequest("Invalid Data");
            }

            var introductionIdDto = new IntroductionHomePageId { Id = introductionHomePageId.Id };
            var result = await _endPointInHomePage.DeleteIntroductionHomePageAsync(introductionIdDto, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }








        //End Point For StartHere Page
        // Add Introduction to Start Here

        [HttpPost("AddIntroductionStartHere")]
        [Authorize(Roles = DefaultRole.Admin)]
        public async Task<IActionResult> AddIntroductionStartHere([FromBody] IntroductionStartHerePageDto  introductionInStartHerePageDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }
            var result = await _endPointInStartHere.AddIntroductionStartHereAsync(introductionInStartHerePageDto, cancellationToken);


            return result.IsSuccess? Ok() : result.ToProblem();
        }

        // Update an Introduction in Start Here
        [HttpPut("UpdateIntroductionStartHere/{Id}")]
        [Authorize(Roles = DefaultRole.Admin)]
        public async Task<IActionResult> UpdateIntroductionInStartHere([FromRoute] IntroductionStartHerePageId introductionInStartHerePageId, [FromBody] IntroductionStartHerePageDto   introductionInStartHerePageDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }
            var introductionStartHereId = new IntroductionStartHerePageId { Id = introductionInStartHerePageId.Id };
            var result = await _endPointInStartHere.UpdateIntroductionInStartHereAsync(introductionStartHereId, introductionInStartHerePageDto, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }

        // Get a Specific Introduction in Start Here by ID
        [HttpGet("GetIntroductionStartHere/{Id}")]
        public async Task<IActionResult> GetIntroductionInStartHere([FromRoute] IntroductionStartHerePageId introductionInStartHerePageId, CancellationToken cancellationToken = default)
        {
            if (introductionInStartHerePageId == null || !ModelState.IsValid)
            {
                return BadRequest("This Field Can not be empty.");
            }
            var result = await _endPointInStartHere.GetIntroductionInStartHereAsync(introductionInStartHerePageId, cancellationToken);
            
            return result.IsSuccess? Ok(result.Value) : result.ToProblem();
        }


        // Get All Introductions in Start Here
        [HttpGet("GetAllIntroductionsStartHere")]
        public async Task<IActionResult> GetAllIntroductionsInStartHere(CancellationToken cancellationToken = default)
        {
            var result = await _endPointInStartHere.GetAllIntroductionInStartHereAsync(cancellationToken);
            return result.IsSuccess?  Ok(result.Value) : result.ToProblem();
        }


        // Delete Introduction in Start Here
        [HttpDelete("DeleteIntroductionStartHere/{Id}")]
        [Authorize(Roles = DefaultRole.Admin)]
        public async Task<IActionResult> DeleteIntroductionInStartHere([FromRoute] IntroductionStartHerePageId introductionInStartHerePageId, CancellationToken cancellationToken = default)
        {
            var introductionSrartHereId = new IntroductionStartHerePageId { Id = introductionInStartHerePageId.Id };
            var result = await _endPointInStartHere.DeleteIntroductionInStartHereAsync(introductionSrartHereId, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }






        [HttpPost("AddImportantnStartHere")]
        [Authorize(Roles = DefaultRole.Admin)]
        public async Task<IActionResult> AddImportantnStartHere([FromBody] ImportantStartHereDto importantStartHereDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }
            var result = await _endPointInStartHere.AddImportantStartHereAsync(importantStartHereDto, cancellationToken);

            if (result == null)
            {
                return Conflict("Invalid Data");
            }

            return result.IsSuccess? Ok() : result.ToProblem() ;
        }


        [HttpPut("UpdateImportantStartHere/{Id}")]
        [Authorize(Roles = DefaultRole.Admin)]
        public async Task<IActionResult> UpdateImportantStartHere([FromRoute] ImportantStartHereId importantStartHereId, [FromBody] ImportantStartHereDto importantStartHereDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }
            var result = await _endPointInStartHere.UpdateImportantStartHereAsync(importantStartHereId, importantStartHereDto, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }

        // Get specific DetailsForCarousel by ID
        [HttpGet("GetImportantStartHere/{Id}")]
        public async Task<IActionResult> GetImportantStartHere([FromRoute] ImportantStartHereId importantStartHereId, CancellationToken cancellationToken = default)
        {
            if (importantStartHereId == null || !ModelState.IsValid)
            {
                return BadRequest("This field cannot be empty.");
            }

            var result = await _endPointInStartHere.GetImportantStartHereAsync(importantStartHereId, cancellationToken);
            if (result == null)
            {
                return NotFound();
            }
            return result.IsSuccess? Ok(result.Value) : result.ToProblem();
        }

        // Get all DetailsForCarousel
        [HttpGet("GetAllImportantStartHere")]
        public async Task<IActionResult> GetAllImportantStartHere(CancellationToken cancellationToken = default)
        {
            var result = await _endPointInStartHere.GetAllImportantStartHereAsync(cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }


        // Delete specific DetailsForCarousel by ID
        [HttpDelete("DeleteImportantStartHere/{Id}")]
        public async Task<IActionResult> DeleteImportantStartHere([FromRoute] ImportantStartHereId importantStartHereId, CancellationToken cancellationToken = default)
        {
            var ImportantDatalId = new ImportantStartHereId { Id = importantStartHereId.Id };
            var result = await _endPointInStartHere.DeleteImportantStartHereAsync(ImportantDatalId, cancellationToken);

            return result.IsSuccess ? Ok() : result.ToProblem();
        }




        // Add Section Type Roadmap
        [HttpPost("AddCarouselSection")]
        [Authorize(Roles = DefaultRole.Admin)]
        public async Task<IActionResult> AddCarouselSection([FromBody] NewCarouselSectionDto sectionTypeRoadmapDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }
            var result = await _endPointInStartHere.AddCarouselSectionAsync(sectionTypeRoadmapDto, cancellationToken);

            if (result == null)
            {
                return Conflict("Invalid Data");
            }

            return result.IsSuccess ? Ok() : result.ToProblem();
        }

      


        // Update Section Type Roadmap
        [HttpPut("UpdateCarouselSection/{Id}")]
        [Authorize(Roles = DefaultRole.Admin)]
        public async Task<IActionResult> UpdateCarouselSection([FromRoute] NewCarouselSectionId sectionTypeRoadmapId, [FromBody] NewCarouselSectionDto sectionTypeRoadmapDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }
            var result = await _endPointInStartHere.UpdateCarouselSectionAsync(sectionTypeRoadmapId, sectionTypeRoadmapDto, cancellationToken);
                return result.IsSuccess ? Ok() : result.ToProblem();
           
        }


        // Get All Section Type Roadmaps
        [HttpGet("GetAllCarouselSection")]
        public async Task<IActionResult> GetAllCarouselSection(CancellationToken cancellationToken = default)
        {
            var result = await _endPointInStartHere.GetAllCarouselSectionAsync(cancellationToken);
            return result.IsSuccess? Ok(result.Value) : result.ToProblem();
        }


        // Get a Specific Section Type Roadmap by ID
        [HttpGet("GetCarouselSection/{Id}")]
        public async Task<IActionResult> GetCarouselSection([FromRoute] NewCarouselSectionId sectionTypeRoadmapId, CancellationToken cancellationToken = default)
        {
            
            var result = await _endPointInStartHere.GetCarouselSectionAsync(sectionTypeRoadmapId, cancellationToken);
           
            return result.IsSuccess? Ok(result.Value) : result.ToProblem();
        }

        // Delete Section Type Roadmap
        [HttpDelete("DeleteCarouselSection/{Id}")]
        [Authorize(Roles = DefaultRole.Admin)]
        public async Task<IActionResult> DeleteCarouselSection([FromRoute] NewCarouselSectionId sectionTypeRoadmapId, CancellationToken cancellationToken = default)
        {

            var introductionSrartHereId = new IntroductionStartHerePageId { Id = sectionTypeRoadmapId.Id };
            var result = await _endPointInStartHere.DeleteCarouselSectionAsync(sectionTypeRoadmapId, cancellationToken);

            return result.IsSuccess ? Ok() : result.ToProblem();

        }





        [HttpPost("AddDetailsCarouselSection")]
        [Authorize(Roles = DefaultRole.Admin)]
        public async Task<IActionResult> AddDetailsCarouselSection([FromBody] DetailsCarouselSectionDto detailsForCarouselDto, CancellationToken cancellationToken = default)
        {

            if (!ModelState.IsValid)
                return BadRequest("Invalid Data");

            var result = await _endPointInStartHere.AddDetailsCarouselSectionAsync(detailsForCarouselDto, cancellationToken);


            return  result.IsSuccess? Ok() : result.ToProblem();
        }


        [HttpPut("UpdateDetailsCarouselSection/{Id}")]
        [Authorize(Roles = DefaultRole.Admin)]
        public async Task<IActionResult> UpdateDetailsCarouselSection([FromRoute] DetailsCarouselSectionId detailsForCarouselId, [FromBody] DetailsCarouselSectionDto detailsForCarouselDto, CancellationToken cancellationToken = default)
        {
             
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }
            var result = await _endPointInStartHere.UpdateDetailsCarouselSectionAsync(detailsForCarouselId, detailsForCarouselDto, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem() ;
        }

        [HttpGet("GetDetailsCarouselSection/{Id}")]
        public async Task<IActionResult> GetDetailsCarouselSection([FromRoute] DetailsCarouselSectionId detailsForCarouselId, CancellationToken cancellationToken = default)
        {
             
           
            var result = await _endPointInStartHere.GetDetailsCarouselSectionAsync(detailsForCarouselId, cancellationToken);
            
            return result.IsSuccess? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("GetAllDetailsCarouselSection")]
        public async Task<IActionResult> GetAllDetailsCarouselSection(CancellationToken cancellationToken = default)
        {
           
            var result = await _endPointInStartHere.GetAllDetailsCarouselSectionAsync(cancellationToken);

            return result.IsSuccess? Ok(result.Value) : result.ToProblem();
        }


        [HttpDelete("DeleteDetailsCarouselSection/{Id}")]
        [Authorize(Roles = DefaultRole.Admin)]
        public async Task<IActionResult> DeleteDetailsCarouselSection([FromRoute] DetailsCarouselSectionId detailsForCarouselId, CancellationToken cancellationToken = default)
        {
            
            var detailsCarouselId = new DetailsCarouselSectionId { Id = detailsForCarouselId.Id };
            var result = await _endPointInStartHere.DeleteDetailsCarouselSectionAsync(detailsCarouselId, cancellationToken);

            return result.IsSuccess ? Ok() : result.ToProblem();
        }








    }



}
