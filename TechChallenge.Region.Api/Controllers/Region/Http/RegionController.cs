using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TechChallenge.Region.Api.Controllers.Region.Dto;
using TechChallenge.Region.Api.Response;
using TechChallenge.Region.Domain.Region.Entity;
using TechChallenge.Region.Domain.Region.Exception;
using TechChallenge.Region.Domain.Region.Service;

namespace TechChallenge.Region.Api.Controllers.Region.Http
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegionController : Controller
    {
        private readonly IRegionService _regionService;
        private readonly IMapper _mapper;

        public RegionController(IRegionService regionService,
                                IMapper mapper)
        {
            _regionService = regionService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] RegionCreateDto regionDto)
        {
            try
            {
                var regionEntity = _mapper.Map<RegionEntity>(regionDto);
                await _regionService.CreateAsync(regionEntity).ConfigureAwait(false);

                return StatusCode(204, new BaseResponse
                {
                    Success = true,
                    Error = string.Empty
                });
            }
            catch (RegionAlreadyExistsException ex)
            {
                return StatusCode(400, new BaseResponse
                {
                    Error = ex.Message,
                    Success = false
                });
            }
            catch (Exception)
            {
                return StatusCode(400, new BaseResponse
                {
                    Error = "Ocorreu um erro!",
                    Success = false
                });
            }
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            try
            {
                var regionEntity = await _regionService.GetByIdWithCacheAsync(id).ConfigureAwait(false);
                var regionDto = _mapper.Map<RegionResponseDto>(regionEntity);

                return StatusCode(200, new BaseResponseDto<RegionResponseDto>
                {
                    Success = true,
                    Error = string.Empty,
                    Data = regionDto
                });
            }
            catch (RegionNotFoundException ex)
            {
                return StatusCode(400, new BaseResponse
                {
                    Error = ex.Message,
                    Success = false
                });
            }
            catch (Exception)
            {
                return StatusCode(400, new BaseResponse
                {
                    Error = "Ocorreu um erro!",
                    Success = false
                });
            }

        }

        [HttpGet("get-by-ddd/{ddd}")]
        public async Task<IActionResult> GetByDdd([FromRoute] string ddd)
        {
            try
            {
                var regionEntity = await _regionService.GetByDdd(ddd).ConfigureAwait(false);

                var regionDto = _mapper.Map<RegionResponseDto>(regionEntity);

                return StatusCode(200, new BaseResponseDto<RegionResponseDto>
                {
                    Success = true,
                    Error = string.Empty,
                    Data = regionDto
                });
            }
            catch (RegionNotFoundException ex)
            {
                return StatusCode(400, new BaseResponseDto<RegionResponseDto>
                {
                    Error = ex.Message,
                    Success = false
                });
            }
            catch (Exception)
            {
                return StatusCode(400, new BaseResponse
                {
                    Error = "Ocorreu um erro!",
                    Success = false
                });
            }

        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllPagedAsync([FromQuery] int pageSize = 10, [FromQuery] int page = 0)
        {
            try
            {
                var regions = await _regionService.GetAllPagedAsync(pageSize, page).ConfigureAwait(false);

                var totalItems = await _regionService.GetCountAsync().ConfigureAwait(false);

                var response = _mapper.Map<IEnumerable<RegionResponseDto>>(regions);


                return StatusCode(200, new BaseResponsePagedDto<IEnumerable<RegionResponseDto>>
                {
                    Success = true,
                    Error = string.Empty,
                    Data = response,
                    CurrentPage = page,
                    TotalItems = totalItems,
                    ItemsPerPage = pageSize
                });
            }
            catch (RegionNotFoundException ex)
            {
                return StatusCode(400, new BaseResponse
                {
                    Error = ex.Message,
                    Success = false
                });
            }
            catch (Exception)
            {
                return StatusCode(400, new BaseResponse
                {
                    Error = "Ocorreu um erro!",
                    Success = false
                });
            }

        }

        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] RegionUpdateDto regionDto)
        {
            try
            {
                var regionEntity = _mapper.Map<RegionEntity>(regionDto);

                await _regionService.UpdateAsync(regionEntity).ConfigureAwait(false);

                return StatusCode(204, new BaseResponse
                {
                    Success = true,
                    Error = string.Empty
                });
            }
            catch (RegionNotFoundException ex)
            {
                return StatusCode(400, new BaseResponse
                {
                    Error = ex.Message,
                    Success = false
                });
            }
            catch (Exception)
            {
                return StatusCode(400, new BaseResponse
                {
                    Error = "Ocorreu um erro!",
                    Success = false
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            try
            {
                await _regionService.DeleteByIdAsync(id).ConfigureAwait(false);

                return StatusCode(204, new BaseResponse
                {
                    Success = true,
                    Error = string.Empty
                });
            }
            catch (RegionNotFoundException ex)
            {
                return StatusCode(400, new BaseResponse
                {
                    Error = ex.Message,
                    Success = false
                });
            }
            catch (Exception)
            {
                return StatusCode(400, new BaseResponse
                {
                    Error = "Ocorreu um erro!",
                    Success = false
                });
            }

        }
    }
}

//test