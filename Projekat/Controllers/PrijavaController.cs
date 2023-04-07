using Projekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Projekat.Controllers
{
    public class PrijavaController : ApiController
    {
		public IHttpActionResult Get(string korisnickoime, string lozinka)
		{
			Korisnik korisnik = KorisniciBaza.Prijava(korisnickoime, lozinka);
			if (korisnik != null)
			{
				return Ok(korisnik);
			}
			else
			{ return BadRequest("Greska pri prijavljivanju."); }
		}
	}
}
