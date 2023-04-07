using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
	public class GrupniTrening
	{
		public string Naziv { get; set; }
		public string TipTreninga { get; set; }
		public string FitnesCentar { get; set; }
		public int Trajanje { get; set; }
		public string Vreme { get; set; }
		public int MaksimalanBrojPosetilaca { get; set; }
		public List<string> SpisakPosetilaca { get; set; } = new List<string>();
		public bool Istekao { get; set; } = false;
	}
}