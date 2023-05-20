using Model.Persons;
using Model.RoadGrids;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Model.Tiles.Buildings
{
	public class PoliceDepartmentBuildingTile : Building, IWorkplace, IHappyZone
	{
		private readonly List<Worker> _workers = new();
		public int WorkplaceLimit { get; private set; }

		/// <summary>
		/// Construct a new police department tile
		/// </summary>
		/// <param name="x">X coordinate of the tile</param>
		/// <param name="y">Y coordinate of the tile</param>
		/// <param name="designID">DesignID for the tile</param>
		/// <param name="rotation">Rotation of the tile</param>
		public PoliceDepartmentBuildingTile(int x, int y, uint designID, Rotation rotation) : base(x, y, designID, rotation)
		{

		}

		public override void FinalizeTile()
		{
			Finalizing();
		}

		/// <summary>
		/// <para>MUST BE STARTED WITH <code>base.Finalizing()</code></para>
		/// <para>Do the actual finalization</para>
		/// </summary>
		protected new void Finalizing()
		{
			base.Finalizing();
			//TODO implement  workplace limit
			WorkplaceLimit = 10;

			for (int i = 0; i <= GetRegisterRadius(); i++)
			for (int j = 0; Mathf.Sqrt(i * i + j * j) <= GetRegisterRadius(); j++)
			{
				if (i == 0 && j == 0) { continue; }

				//register at the residentials
				if (City.Instance.GetTile(Coordinates.x + i, Coordinates.y - j) is IResidential residentialTopRight) { residentialTopRight.RegisterHappinessChangerTile(this); }
				if (City.Instance.GetTile(Coordinates.x + i, Coordinates.y + j) is IResidential residentialBottomRight && j != 0) { residentialBottomRight.RegisterHappinessChangerTile(this); }
				if (City.Instance.GetTile(Coordinates.x - i, Coordinates.y + j) is IResidential residentialBottomLeft && j != 0) { residentialBottomLeft.RegisterHappinessChangerTile(this); }
				if (City.Instance.GetTile(Coordinates.x - i, Coordinates.y - j) is IResidential residentialTopLeft) { residentialTopLeft.RegisterHappinessChangerTile(this); }

				//register at the workplaces
				if (City.Instance.GetTile(Coordinates.x + i, Coordinates.y - j) is IWorkplace workplaceTopRight) { workplaceTopRight.RegisterHappinessChangerTile(this); }
				if (City.Instance.GetTile(Coordinates.x + i, Coordinates.y + j) is IWorkplace workplaceBottomRight && j != 0) { workplaceBottomRight.RegisterHappinessChangerTile(this); }
				if (City.Instance.GetTile(Coordinates.x - i, Coordinates.y + j) is IWorkplace workplaceBottomLeft && j != 0) { workplaceBottomLeft.RegisterHappinessChangerTile(this); }
				if (City.Instance.GetTile(Coordinates.x - i, Coordinates.y - j) is IWorkplace workplaceTopLeft) { workplaceTopLeft.RegisterHappinessChangerTile(this); }

				//register to the destroy event to be notified about a new tile
				if (City.Instance.GetTile(Coordinates.x + i, Coordinates.y - j) is Tile aboveTile) { aboveTile.OnTileDelete += TileDestroyedInRadius; }
				if (City.Instance.GetTile(Coordinates.x + i, Coordinates.y + j) is Tile rightTile && j != 0) { rightTile.OnTileDelete += TileDestroyedInRadius; }
				if (City.Instance.GetTile(Coordinates.x - i, Coordinates.y + j) is Tile bottomTile && j != 0) { bottomTile.OnTileDelete += TileDestroyedInRadius; }
				if (City.Instance.GetTile(Coordinates.x - i, Coordinates.y - j) is Tile leftTile) { leftTile.OnTileDelete += TileDestroyedInRadius; }
			}
		}

		/// <summary>
		/// Register at and to the new tile
		/// </summary>
		/// <param name="oldTile">Old tile that was deletetd</param>
		private void TileDestroyedInRadius(object sender, Tile oldTile)
		{
			Tile newTile = City.Instance.GetTile(oldTile);

			if (newTile is IResidential residential)	{ residential.RegisterHappinessChangerTile(this); }
			if (newTile is IWorkplace workplace)		{ workplace.RegisterHappinessChangerTile(this); } //TODO
			newTile.OnTileDelete += TileDestroyedInRadius;
		}

		public override TileType GetTileType() { return TileType.PoliceDepartment; }

		public void RegisterWorkplace(RoadGrid roadGrid)
		{
			if (!_isFinalized) { throw new InvalidOperationException(); }

			roadGrid?.AddWorkplace(this);
		}

		public void UnregisterWorkplace(RoadGrid roadGrid)
		{
			if (!_isFinalized) { throw new InvalidOperationException(); }

			roadGrid?.RemoveWorkplace(this);
		}

		public void Employ(Worker person)
		{
			if (!_isFinalized) { throw new InvalidOperationException(); }

			if (GetWorkersCount() >= WorkplaceLimit) { throw new InvalidOperationException("The workplace is full"); }

			if (GetWorkersCount() == 0)
			{
				TileChangeInvoke();
			}

			_workers.Add(person);
		}

		public void Unemploy(Worker person)
		{
			if (!_isFinalized) { throw new InvalidOperationException(); }

			_workers.Remove(person);

			if (GetWorkersCount() == 0)
			{
				TileChangeInvoke();
			}
		}

		public List<Worker> GetWorkers()
		{
			if (!_isFinalized) { throw new InvalidOperationException(); }

			return _workers;
		}

		public int GetWorkersCount()
		{
			if (!_isFinalized) { throw new InvalidOperationException(); }

			return _workers.Count;
		}

		public override int GetBuildPrice()
		{
			//TODO implement police department build price
			return 100000;
		}

		public override int GetDestroyIncome()
		{
			//TODO implement police department destroy income
			return 100000;
		}

		public override int GetMaintainanceCost()
		{
			//TODO implement police department maintainance cost
			return 100000;
		}

		private int GetRegisterRadius()
		{
			return 5;
		}

		public int GetEffectiveRadius()
		{
			return GetWorkersCount() > 0 ? GetRegisterRadius() : 0;
		}

		public (float happiness, float weight) GetHappinessModifierAtTile(Building building)
		{
			if (!_isFinalized) { throw new InvalidOperationException(); }

			if (building == null) { throw new ArgumentNullException(); }
			if (building == this) { throw new ArgumentException("Target can't be same as this"); }

			//it is not even in the same grid
			if (RoadGridManager.GetRoadGrigElementByBuilding(this) == null) { return (0, 0); }
			if (RoadGridManager.GetRoadGrigElementByBuilding(building) == null) { return (0, 0); }
			if (RoadGridManager.GetRoadGrigElementByBuilding(building).RoadGrid != RoadGridManager.GetRoadGrigElementByBuilding(this).RoadGrid) { return (0, 0); }

			//it is not reachable
			int distance = GetDistanceOnRoad(RoadGridManager.GetRoadGrigElementByBuilding(building), GetEffectiveRadius() - 1);
			if (distance == -1)
			{
				return (0, 0);
			}

			return (1, Mathf.Cos(distance * Mathf.PI / 2 / GetEffectiveRadius()));
		}

		public int GetDistanceOnRoad(IRoadGridElement target, int maxDistance)
		{
			Queue<(IRoadGridElement, int)> queue = new();
			queue.Enqueue((RoadGridManager.GetRoadGrigElementByBuilding(this), 0));
			while (queue.Count > 0)
			{
				(IRoadGridElement roadGridElement, int distance) = queue.Dequeue();

				if (roadGridElement == target) return distance;

				if (distance < maxDistance)
				{
					foreach (IRoadGridElement element in roadGridElement.ConnectsTo)
					{
						queue.Enqueue((element, distance + 1));
					}
				}
			}

			return -1;
		}

		public (float happiness, float weight) HappinessByBuilding
		{
			get
			{
				float happinessSum = _happinessChangers.Aggregate(0.0f, (acc, item) => acc + item.happiness * item.weight);
				float happinessWeight = _happinessChangers.Aggregate(0.0f, (acc, item) => acc + item.weight);
				return (happinessSum / (happinessWeight == 0 ? 1 : happinessWeight), happinessWeight);
			}
		}

		private readonly List<(IHappyZone happyZone, float happiness, float weight)> _happinessChangers = new();
		public void RegisterHappinessChangerTile(IHappyZone happyZone)
		{
			happyZone.GetTile().OnTileDelete += UnregisterHappinessChangerTile;
			happyZone.GetTile().OnTileChange += UpdateHappiness;

			(float happiness, float weight) = happyZone.GetHappinessModifierAtTile(this);
			_happinessChangers.Add((happyZone, happiness, weight));
		}

		private void UnregisterHappinessChangerTile(object sender, Tile deletedTile)
		{
			IHappyZone happyZone = (IHappyZone)deletedTile;
			_happinessChangers.RemoveAll((values) => values.happyZone == happyZone);
		}

		private void UpdateHappiness(object sender, Tile changedTile)
		{
			IHappyZone happyZone = (IHappyZone)changedTile;
			_happinessChangers.RemoveAll((values) => values.happyZone == happyZone);

			(float happiness, float weight) = happyZone.GetHappinessModifierAtTile(this);
			_happinessChangers.Add((happyZone, happiness, weight));
		}

		public override float GetTransparency()
		{
			return 0.75f;
		}
	}
}