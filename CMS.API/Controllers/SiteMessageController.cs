using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CMS.Contracts;
using CMS.DTO;
using CMS.Entities;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteMessageController : ControllerBase
    {
        private readonly MapperConfiguration _mapperConfiguration;
        private readonly ISiteMessageRepository _siteMessage;
        private readonly UserManager<User> _userManager;

        public SiteMessageController(MapperConfiguration mapperConfiguration,ISiteMessageRepository siteMessage , UserManager<User> userManager)
        {
            _mapperConfiguration = mapperConfiguration;
            _siteMessage = siteMessage;
            _userManager = userManager;
        }
        // GET: api/SiteMessage
        [HttpGet]
        [Authorize]
        public IEnumerable<SiteMessageDTO> Get()
        {
            int userId = _userManager.GetUserId(User).To<int>();
            var siteMessageModel = _siteMessage.AllUserMessages(userId).ToList();
            var siteMessageDTO = _mapperConfiguration.CreateMapper().Map<List<SiteMessageDTO>>(siteMessageModel);
            return siteMessageDTO;
        }

        // GET: api/SiteMessage/5
        [HttpGet("{id}")]
        [Route("GetSingle/{id}")]
        public IActionResult GetSingle(int id)
        {
            var siteMessageModel = _siteMessage.GetById(id);
            var siteMessageDTO = _mapperConfiguration.CreateMapper().Map<SiteMessageDTO>(siteMessageModel);
            return Ok(siteMessageDTO);
        }

        // POST: api/SiteMessage
        [HttpPost]
        public IActionResult Post([FromBody] SiteMessageDTO siteMessageDto)
        {
            var insertModel = _mapperConfiguration.CreateMapper().Map<SiteMessage>(siteMessageDto);
            _siteMessage.Add(insertModel);
            return CreatedAtAction(nameof(Get), new { id = insertModel.Id }, siteMessageDto);
        }

        // PUT: api/SiteMessage/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
