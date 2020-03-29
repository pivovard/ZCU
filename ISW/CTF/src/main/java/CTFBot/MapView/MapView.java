package CTFBot.MapView;

import cz.cuni.amis.pathfinding.map.IPFMapView;
import cz.cuni.amis.pogamut.ut2004.agent.module.sensor.Players;
import cz.cuni.amis.pogamut.ut2004.agent.module.sensor.visibility.Visibility;
import cz.cuni.amis.pogamut.ut2004.communication.messages.gbinfomessages.NavPoint;

import java.util.Collection;

/**
 * Created by pivov on 09-May-19.
 */
public class MapView implements IPFMapView<NavPoint> {

    Visibility visibility;
    Players players;

    public MapView(Players players, Visibility visibility){
        this.players = players;
        this.visibility = visibility;
    }

    @Override
    public Collection<NavPoint> getExtraNeighbors(NavPoint node, Collection<NavPoint> mapNeighbors) {
        return null;
    }

    @Override
    public int getNodeExtraCost(NavPoint node, int mapCost) {
        return 0;
    }

    @Override
    public int getArcExtraCost(NavPoint nodeFrom, NavPoint nodeTo, int mapCost) {
        return 0;
    }

    @Override
    public boolean isNodeOpened(NavPoint node) {
        return true;
    }

    @Override
    public boolean isArcOpened(NavPoint nodeFrom, NavPoint nodeTo) {
        return true;
    }

}