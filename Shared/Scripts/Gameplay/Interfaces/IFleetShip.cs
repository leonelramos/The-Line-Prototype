using System.Collections.Generic;
public interface IFleetShip
{
	List<IFleetShip> AllyFleet { set; }
	List<IFleetShip> EnemyFleet { set; }
}
