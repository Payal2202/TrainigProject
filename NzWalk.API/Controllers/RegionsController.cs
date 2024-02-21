using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NzWalk.API.Data;
using NzWalk.API.Model.Domain;
using NzWalk.API.Model.DTO;
using NzWalk.API.Repositories;

namespace NzWalk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _context;
        private readonly IRegionRepository regionRepository;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository)
        {
            this._context = dbContext;
            this.regionRepository = regionRepository;
        }

        // Get data for all regions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            //var regionsDomain = await _context.Regions.ToListAsync();

            var regionsDomain = await regionRepository.GetAllAsync();

            var regionDto = new List<RegionDto>();
            foreach (var regionDomain in regionsDomain)
            {
                regionDto.Add(new RegionDto()
                {
                    Id = regionDomain.Id,
                    Name = regionDomain.Name,
                    Code = regionDomain.Code,
                    RegionImageUrl = regionDomain.RegionImageUrl
                });
            }
            return Ok(regionDto);
        }


        // Get data for all regions by id
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            // var region = _context.Regions.Find(id);

            var regionDomain = await _context.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            var regionDto = new RegionDto
            {
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                Code = regionDomain.Code,
                RegionImageUrl = regionDomain.RegionImageUrl
            };

            return Ok(regionDto);
        }

        // Create region
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // Convert DTO to DomainModel
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

            await _context.Regions.AddAsync(regionDomainModel);
            await _context.SaveChangesAsync();

            // Convert DomainModel to DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionDto);
        }

        //Update region by id
        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id,UpdateRegionRequestDto updateRegionRequestDto)
        {
            var regionDomainModel = await _context.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            regionDomainModel.Code = updateRegionRequestDto.Code;
            regionDomainModel.Name = updateRegionRequestDto.Name;
            regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

            await _context.SaveChangesAsync();

            // Convert DomainModel to DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto);
        }

        //Delete region by id
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await _context.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            _context.Regions.Remove(regionDomainModel);
            await _context.SaveChangesAsync();

            // Convert DomainModel to DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto);
        }
    }
}
