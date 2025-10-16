using Azure.Core;
using BlobStorage.Core.DTOs.Blob;
using BlobStorage.Core.Global;
using BlobStorage.Core.Interfaces;
using BlobStorage.Core.Interfaces.BlobMetdata;
using BlobStorage.Core.Models;
using BlobStorage.Service;
using BlobStorage.Service.DTOs.Blob;
using BlobStorage.Service.DTOs.Blobmetadata;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlobStorage.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlobController : ControllerBase
    {

        private readonly BlobMetadataService _BlobMetadataService;
        private readonly IObjectStorage _BlobRepository;
        private readonly BlobService _BlobService;

        public BlobController(BlobMetadataService BlobMetadataService, BlobService BlobService)
        {
            _BlobMetadataService = BlobMetadataService; 
            _BlobService = BlobService; 
        }




        [Authorize]
        [HttpPost("/v1/blobs", Name = "/v1/blobs")] 
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BlobDTO>> AddBlob([FromBody] AddBlob EncodedData )
        {
            var curentUserId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            
            if (EncodedData == null  )
            {
                return BadRequest("Base64 data is required.");
            }

          
            string cleanBase64 = EncodedData.Data
                .Replace("\r", "")
                .Replace("\n", "")
                .Trim();

            var BlobDto =  await _BlobService.UploadAsync(cleanBase64, Convert.ToInt32( curentUserId) );

         
            return CreatedAtRoute("/v1/blobs", new {  id = BlobDto.Id }, new { id = BlobDto.Id, message = "data has Successfully saved !" });


        }
        




        [Authorize]
        [HttpGet("/v1/blobs{Id}", Name = "getBlob")] 
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RetrieveBlobDTO>> AddBlob(Guid Id)
        {
        
            var BlobData = await _BlobService.RetrieveBlobAsyn(Id);
            if(BlobData == null)
            {
                return NotFound("Blob data does not exist !");
            }
           
            return Ok(BlobData);

        }




    }
}
