using Projekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Projekat.Controllers
{
    public class RegistracijaController : ApiController
    {
		public IHttpActionResult Post(Korisnik korisnik)
		{
			
			if (ProveraUnosa(korisnik))
			{
				if (KorisniciBaza.Registracija(korisnik))
				{
					return Ok(korisnik);
				}
				else
				{ return BadRequest("Greska pri registraciji."); }
			}
			return BadRequest("Polja nisu dobro popunjena!");
		}

		[NonAction]
		public bool ProveraUnosa(Korisnik korisnik)
		{
			if(String.IsNullOrWhiteSpace(korisnik.KorisnickoIme) || String.IsNullOrWhiteSpace(korisnik.Lozinka) || String.IsNullOrWhiteSpace(korisnik.Ime) || String.IsNullOrWhiteSpace(korisnik.Prezime) || String.IsNullOrWhiteSpace(korisnik.Pol) || String.IsNullOrWhiteSpace(korisnik.Email) || String.IsNullOrWhiteSpace(korisnik.DatumRodjenja))
			{
				return false;
			}
			return true;
		}

	}
}
