using System.Collections.Generic;

namespace Model
{
	public interface IResidential
	{
		public void RegisterHome();
		public void UnregisterHome();

		public bool MoveIn(Person person);
		public bool MoveOut(Person person);
		public List<Person> GetResidents();
		public int GetResidentsCount();
		public int GetResidentsLimit();

		public Tile GetTile();
	}
}