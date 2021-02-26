using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilibusterSimulator.BusinessLogic
{
	public class Save
	{
		public string Name { get; set; }
		public int Slot { get; set; }
		public Person Person { get; set; }
		public Game Game { get; set; }

		public static List<Save> GetAllSaves()
		{
			string json = "";
			using (StreamReader sr = new StreamReader("saves.json"))
			{
				json = sr.ReadToEnd();
			}

			List<Save> saves = new List<Save>();

			JsonTextReader reader = new JsonTextReader(new StringReader(json));
			reader.SupportMultipleContent = true;

			while (reader.Read())
			{
				JsonSerializer serializer = new JsonSerializer();
				Save save = serializer.Deserialize<Save>(reader);

				saves.Add(save);
			}

			return saves;
		}
	}
}
