using AutoMapper;
using DataModels.DTOs;
using DataModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QuoteQuizApi.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace QuoteQuizApi.Controllers
{
    [Route("api/Quotes")]
    [ApiController]
    public class QuoteController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        public QuoteController(IUnitOfWork unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpGet]
        public async Task<IActionResult> GetQuotesAsync()
        {
            IEnumerable<Quote> quotes = await _unitOfWork.QuoteRepository.GetAllAsync();
            IEnumerable<QuoteDto> dtos = _mapper.Map<IList<QuoteDto>>(quotes);
            //Request.HttpContext.Response.Headers.Add("total count", dtos.Count().ToString());
            return Ok(dtos);
        }
        [HttpGet("{id}", Name = nameof(GetQuoteByIdAsync))]
        public async Task<IActionResult> GetQuoteByIdAsync(int id)
        {

            var quote = await _unitOfWork.QuoteRepository.GetAsync(id);
            QuoteDto dto = _mapper.Map<QuoteDto>(quote);
            if (dto == null)
            {
                return NotFound();
            }

            return new ObjectResult(dto)
            {
                StatusCode = (int)HttpStatusCode.OK
            };
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuoteAsync([FromBody] QuoteDto quoteDTO)
        {
            var quote = _mapper.Map<Quote>(quoteDTO);
            var addedQuote = _unitOfWork.QuoteRepository.Add(quote);
            await _unitOfWork.SaveChangesAsync();
            var action = CreatedAtRoute(nameof(GetQuoteByIdAsync), new { id = addedQuote.Id }, addedQuote.Id);
            return action;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateQuoteAsync([FromBody] QuoteDto quoteDTO)
        {
            var quoteToBeUpdatedInDB = await _unitOfWork.QuoteRepository.GetAsync(quoteDTO.Id);
            if (quoteToBeUpdatedInDB == null)
            {
                return BadRequest("Quote was not found");
            }
            var quote = _mapper.Map<Quote>(quoteDTO);
            quoteToBeUpdatedInDB.CreatorId = quoteDTO.CreatorId;
            quoteToBeUpdatedInDB.Contetnt = quoteDTO.Contetnt;
            var answers = _mapper.Map<IList<QuoteAnswer>>(quoteDTO.Answers).ToList();
            quoteToBeUpdatedInDB.Answers = answers;
            var res = _unitOfWork.QuoteRepository.Update(quoteToBeUpdatedInDB);
            await _unitOfWork.SaveChangesAsync();
            var action = CreatedAtRoute(nameof(GetQuoteByIdAsync), new { id = res.Id }, res.Id);
            return action;
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuoteAsync([FromRoute] int id)
        {
            var quote = await _unitOfWork.QuoteRepository.GetAsync(id);
            if (quote == null)
            {
                return BadRequest("Quote was not found");
            }
            _unitOfWork.QuoteRepository.Remove(quote);
            await _unitOfWork.SaveChangesAsync();
            return Ok();

        }



    }
}
