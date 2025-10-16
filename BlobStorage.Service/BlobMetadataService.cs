using AutoMapper;
using BlobStorage.Core.DTOs.Blob;
using BlobStorage.Core.Global;
using BlobStorage.Core.Interfaces;
using BlobStorage.Core.Interfaces.BlobMetdata;
using BlobStorage.Core.Interfaces.user;
using BlobStorage.Core.Models;
using BlobStorage.Service.DTOs.Blobmetadata;
using BlobStorage.Service.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.Service
{
    public  class BlobMetadataService
    {


        private readonly IBlobMetadataRepository<BlobMetadata> _BlobMetadataRepository;
        private readonly IObjectStorage _objectStorage;
        private readonly IMapper _Mapper;

        public BlobMetadataService(IBlobMetadataRepository<BlobMetadata> BlobMetadataRepository, IMapper mapper ,IObjectStorage objectStorage)
        {
            _BlobMetadataRepository = BlobMetadataRepository;
            _Mapper = mapper;
            _objectStorage = objectStorage;
        }

      
        public async Task<BlobMetadataDTO> GetMetadataByBlobIDAsync(Guid Id)
        {
            var BlobMetadata = await _BlobMetadataRepository.GetMetadataByBlobIdAsync(Id);

            return BlobMetadata;
        }

        public async Task AddBlobmetadataAsync(AddBlobMetadataDTO BlobMetadataDto)
        {
            
       
            var BlobMetadataEntity = _Mapper.Map<BlobMetadata>(BlobMetadataDto);

            await _BlobMetadataRepository.AddAsync(BlobMetadataEntity);
        }

        public async Task<BlobMetadataDTO> GetByIdAsync(int id)
        {
            var BlobMetadataEntity = await _BlobMetadataRepository.GetByIdAsync(id);
            if (BlobMetadataEntity == null)
            {
                return null;
            }

            return _Mapper.Map<BlobMetadataDTO>(BlobMetadataEntity);
        }

       

    }
}
