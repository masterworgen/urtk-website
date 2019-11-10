﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WebSite.Domains;

namespace WebSite.Controllers
{
    [ApiController]
    [Route("document")]
    public class DocumentController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public DocumentController(DatabaseContext context,IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        [Route("GetDocument")]
        public DocumentDomain GetDocument(int documentId)
        {
            return _context.DocumentDomains.FirstOrDefault(domain => domain.Id == documentId);
        }

        [HttpGet]
        [Route("GetDocuments")]
        public List<DocumentDomain> GetDocuments()
        {
            return _context.DocumentDomains.ToList();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [Route("UploadDocument")]
        public DocumentDomain UploadDocument([FromForm]DocumentDomain documentDomain)
        {
            var file = HttpContext.Request.Form.Files.First();
            var uploads = Path.Combine(_hostingEnvironment.ContentRootPath, "uploads/document");
            if (file.Length > 0)
            {
                using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                documentDomain.DocumentLink = "uploads/document/" + file.FileName;
                _context.DocumentDomains.Add(documentDomain);
                _context.SaveChanges();

                return documentDomain;
            }
            return null;
        }
    }
}
