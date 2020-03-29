package CTFBot.MapView;

import cz.cuni.amis.pathfinding.map.IPFMapView;
import cz.cuni.amis.pogamut.ut2004.agent.module.sensor.Players;
import cz.cuni.amis.pogamut.ut2004.agent.module.sensor.visibility.Visibility;
import cz.cuni.amis.pogamut.ut2004.communication.messages.gbinfomessages.NavPoint;
import cz.cuni.amis.pogamut.ut2004.communication.messages.gbinfomessages.Player;

import java.util.Collection;

/**
 * Created by pivov on 09-May-19.
 */
public class CoverMapView implements IPFMapView<NavPoint> {

    Visibility visibility;
    Players players;

    public CoverMapView(Players players, Visibility visibility){
        this.players = players;
        this.visibility = visibility;
    }

    @Override
    public Collection<NavPoint> getExtraNeighbors(NavPoint node, Collection<NavPoint> mapNeighbors) {
        return null;
    }

    @Override
    public int getNodeExtraCost(NavPoint node, int mapCost) {
        int extraCost = 0;
        for (Player player : players.getVisibleEnemies().values()) {
            if (visibility.isVisible(player, node)) {
                extraCost += 500;
            }
        }
        return extraCost;
    }

    @Override
    public int getArcExtraCost(NavPoint nodeFrom, NavPoint nodeTo, int mapCost) {
        return 0;
    }

    @Override
    public boolean isNodeOpened(NavPoint node) {
        // ALL NODES ARE OPENED
        return true;
    }

    @Override
    public boolean isArcOpened(NavPoint nodeFrom, NavPoint nodeTo) {
        // ALL ARCS ARE OPENED
        return true;
    }

}
