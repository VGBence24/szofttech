using Model.Simulation;
using Model.Tiles.Buildings;

namespace Model.Tiles.ZoneCommands
{
	public class MarkZoneCommand : IExecutionCommand
	{
		private readonly int _x;
		private readonly int _y;
		private readonly ZoneType _zoneType;
		private readonly uint _designID;

		public MarkZoneCommand(int x, int y, ZoneType zoneType)
		{;
			_x = x;
			_y = y;
			_zoneType = zoneType;
			_designID = ResidentialBuildingTile.GenerateResidential(0);
		}

		public void Execute()
		{
			if (!(_zoneType == ZoneType.NoZone
			  || SimEngine.Instance.GetTile(_x - 1, _y) is RoadTile || SimEngine.Instance.GetTile(_x + 1, _y) is RoadTile
			  || SimEngine.Instance.GetTile(_x, _y - 1) is RoadTile || SimEngine.Instance.GetTile(_x, _y + 1) is RoadTile))
			{ return; }

			switch (_zoneType)
			{
				case ZoneType.IndustrialZone:
					if (SimEngine.Instance.GetTile(_x, _y) is not EmptyTile) { break; }
					SimEngine.Instance.SetTile(_x, _y, new Industrial(_x, _y, _designID)); // TODO update to IndustrialBuildingTIle
					break;
				case ZoneType.CommercialZone:
					if (SimEngine.Instance.GetTile(_x, _y) is not EmptyTile) { break; }
					SimEngine.Instance.SetTile(_x, _y, new Commercial(_x, _y, _designID)); // TODO update to CommercialBuildingTile
					break;
				case ZoneType.ResidentialZone:
					if (SimEngine.Instance.GetTile(_x, _y) is not EmptyTile) { break; }
					SimEngine.Instance.SetTile(_x, _y, new ResidentialBuildingTile(_x, _y, _designID));
					break;
				case ZoneType.NoZone:
					if (SimEngine.Instance.GetTile(_x, _y) is not Industrial && SimEngine.Instance.GetTile(_x, _y) is not Commercial && SimEngine.Instance.GetTile(_x, _y) is not ResidentialBuildingTile) { break; }
					SimEngine.Instance.SetTile(_x, _y, new EmptyTile(_x, _y, _designID));
					break;
				default:
					break;
			}
		}
	}
}