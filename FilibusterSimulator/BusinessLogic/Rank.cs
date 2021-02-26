using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilibusterSimulator.BusinessLogic
{
	public struct Rank
	{
		public int Id;
		public string Name;

		public Rank(string name)
		{
			Name = name;

			switch (name)
			{
				case "Юнга": Id = 1; break;
				case "Пират": Id = 2; break;
				case "Боцман": Id = 3; break;
				case "Квартмейстер": Id = 4; break;
				case "Старпом": Id = 5; break;
				case "Капитан": Id = 6; break;
				default: throw new ArgumentException($"Нет ранга {name}");
			}
		}

		public static string GetRankNameById(int id)
		{
			switch (id)
			{
				case 1: return "Юнга";
				case 2: return "Пират";
				case 3: return "Боцман";
				case 4: return "Квартмейстер";
				case 5: return "Старпом";
				case 6: return "Капитан";
				default: throw new ArgumentException($"Нет ранга с id {id}");
			}
		}
	}
}
