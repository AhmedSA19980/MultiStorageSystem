using BlobStorage.Service;
using BlobStorage.Service.DTOs.Blobmetadata;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlobStorage.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlobMetadataController : ControllerBase
    {

        private readonly BlobMetadataService _blobMetadataService;

        public BlobMetadataController(BlobMetadataService blobMetadataService)
        {
            _blobMetadataService = blobMetadataService;     
        }

        [Authorize]
        [HttpGet("blobMetadata", Name = "GetBlobMetadata")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BlobMetadataDTO>> GetBlobMetadata(Guid blobId)
        {
            var blobMetadata = await _blobMetadataService.GetMetadataByBlobIDAsync(blobId);

            if (blobMetadata == null)
                return NotFound($"Blob metadata with ID {blobId} not found.");

            return Ok(blobMetadata);
        }
    }
}
