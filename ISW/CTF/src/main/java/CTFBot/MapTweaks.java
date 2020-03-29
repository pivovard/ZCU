package CTFBot;

import cz.cuni.amis.pogamut.ut2004.agent.module.sensor.NavigationGraphBuilder;

/**
 * Class containing adjustments for navigation graph of PogamutCup competition maps.
 * 
 * @author Jimmy
 */
public class MapTweaks {

	/**
	 * Called from {@link CTFBot3#botInitialized(cz.cuni.amis.pogamut.ut2004.communication.messages.gbinfomessages.GameInfo, cz.cuni.amis.pogamut.ut2004.communication.messages.gbinfomessages.ConfigChange, cz.cuni.amis.pogamut.ut2004.communication.messages.gbinfomessages.InitedMessage)}.
	 * @param navBuilder
	 */
	public static void tweak(NavigationGraphBuilder navBuilder) {
		if (navBuilder.isMapName("DM-1on1-Roughinery-FPS")) tweakDM1on1RoughineryFPS(navBuilder);
		if (navBuilder.isMapName("CTF-1on1-Joust")) tweakCTF1on1Joust(navBuilder);
		if (navBuilder.isMapName("CTF-DoubleDammage")) tweakCTFDoubleDammage(navBuilder);
		if (navBuilder.isMapName("CTF-January")) tweakCTFJanuary(navBuilder);
		if (navBuilder.isMapName("CTF-Lostfaith")) tweakCTFLostfaith(navBuilder);
		
	}
	
	// ======================
	// DM-1on1-Roughinery-FPS
	// ======================
	
	private static void tweakDM1on1RoughineryFPS(NavigationGraphBuilder navBuilder) {
	}
	
	// ======================
	// CTF-1on1-Joust
	// ======================
	
	private static void tweakCTF1on1Joust(NavigationGraphBuilder navBuilder) {
		navBuilder.modifyNavPoint("xRedFlagBase0").addZ(-45).apply();
		navBuilder.modifyNavPoint("xBlueFlagBase0").addZ(-45).apply();
		navBuilder.modifyNavPoint("PathNode1").addX(600).apply();
		navBuilder.modifyNavPoint("PathNode3").addX(-200).apply();
		navBuilder.removeEdgesBetween("PathNode0", "PathNode2");
		navBuilder.removeEdgesBetween("PathNode1", "PathNode2");		
		navBuilder.modifyNavPoint("xBlueFlagBase0").modifyEdgeTo("PlayerStart1").clearFlags();
		navBuilder.modifyNavPoint("xBlueFlagBase0").modifyEdgeTo("InventorySpot7").clearFlags();
		navBuilder.modifyNavPoint("xBlueFlagBase0").modifyEdgeTo("PlayerStart8").clearFlags();
		navBuilder.modifyNavPoint("xBlueFlagBase0").modifyEdgeTo("PathNode1").clearFlags();
		navBuilder.modifyNavPoint("xRedFlagBase0").modifyEdgeTo("PathNode3").clearFlags();
		navBuilder.modifyNavPoint("xRedFlagBase0").modifyEdgeTo("InventorySpot5").clearFlags();
		navBuilder.modifyNavPoint("xRedFlagBase0").modifyEdgeTo("InventorySpot4").clearFlags();
		navBuilder.modifyNavPoint("xRedFlagBase0").modifyEdgeTo("PlayerStart0").clearFlags();		
		navBuilder.modifyNavPoint("PlayerStart1").modifyEdgeTo("xBlueFlagBase0").clearFlags();
		navBuilder.modifyNavPoint("InventorySpot7").modifyEdgeTo("xBlueFlagBase0").clearFlags();
		navBuilder.modifyNavPoint("PlayerStart8").modifyEdgeTo("xBlueFlagBase0").clearFlags();
		navBuilder.modifyNavPoint("PathNode1").modifyEdgeTo("xBlueFlagBase0").clearFlags();
		navBuilder.modifyNavPoint("PathNode3").modifyEdgeTo("xRedFlagBase0").clearFlags();
		navBuilder.modifyNavPoint("InventorySpot5").modifyEdgeTo("xRedFlagBase0").clearFlags();
		navBuilder.modifyNavPoint("InventorySpot4").modifyEdgeTo("xRedFlagBase0").clearFlags();
		navBuilder.modifyNavPoint("PlayerStart0").modifyEdgeTo("xRedFlagBase0").clearFlags();
	}
	
	private static void tweakCTFMaul(NavigationGraphBuilder navBuilder) {
        navBuilder.removeEdge("CTF-Maul.PathNode10", "CTF-Maul.JumpSpot6");
        navBuilder.removeEdge("CTF-Maul.PathNode12", "CTF-Maul.JumpSpot6");
        navBuilder.removeEdge("CTF-Maul.PathNode143", "CTF-Maul.JumpSpot6");
        navBuilder.removeEdge("CTF-Maul.PathNode63", "CTF-Maul.JumpSpot6");
        navBuilder.removeEdge("CTF-Maul.PathNode142", "CTF-Maul.JumpSpot6");
        navBuilder.removeEdge("CTF-Maul.PathNode10", "CTF-Maul.JumpSpot20");
        navBuilder.removeEdge("CTF-Maul.PathNode63", "CTF-Maul.JumpSpot20");
        navBuilder.removeEdge("CTF-Maul.PathNode6", "CTF-Maul.JumpSpot20");
        navBuilder.removeEdge("CTF-Maul.PathNode64", "CTF-Maul.JumpSpot20");
        navBuilder.removeEdge("CTF-Maul.InventorySpot801", "CTF-Maul.JumpSpot20");
        navBuilder.removeEdge("CTF-Maul.PlayerStart1", "CTF-Maul.JumpSpot7");
        navBuilder.removeEdge("CTF-Maul.InventorySpot801", "CTF-Maul.JumpSpot7");
        navBuilder.removeEdge("CTF-Maul.PathNode48", "CTF-Maul.JumpSpot7");
        navBuilder.removeEdge("CTF-Maul.PathNode64", "CTF-Maul.JumpSpot7");
        navBuilder.removeEdge("CTF-Maul.PlayerStart8", "CTF-Maul.JumpSpot2");
        navBuilder.removeEdge("CTF-Maul.InventorySpot807", "CTF-Maul.JumpSpot2");
        navBuilder.removeEdge("CTF-Maul.PathNode92", "CTF-Maul.JumpSpot2");
        navBuilder.removeEdge("CTF-Maul.PathNode66", "CTF-Maul.JumpSpot2");
        navBuilder.removeEdge("CTF-Maul.PathNode67", "CTF-Maul.JumpSpot2");
        navBuilder.removeEdge("CTF-Maul.PathNode66", "CTF-Maul.JumpSpot18");
        navBuilder.removeEdge("CTF-Maul.PathNode93", "CTF-Maul.JumpSpot18");
        navBuilder.removeEdge("CTF-Maul.PathNode67", "CTF-Maul.JumpSpot18");
        navBuilder.removeEdge("CTF-Maul.PathNode76", "CTF-Maul.JumpSpot18");
        navBuilder.removeEdge("CTF-Maul.PathNode95", "CTF-Maul.JumpSpot18");
        navBuilder.removeEdge("CTF-Maul.PathNode67", "CTF-Maul.JumpSpot3");
        navBuilder.removeEdge("CTF-Maul.PathNode77", "CTF-Maul.JumpSpot3");
        navBuilder.removeEdge("CTF-Maul.PathNode95", "CTF-Maul.JumpSpot3");
        navBuilder.removeEdge("CTF-Maul.PathNode78", "CTF-Maul.JumpSpot3");
        navBuilder.removeEdge("CTF-Maul.PathNode96", "CTF-Maul.JumpSpot3");
	}
	
	private static void tweakCTFCitadel(NavigationGraphBuilder navBuilder) {
        navBuilder.removeEdgesBetween("CTF-Citadel.PathNode99", "CTF-Citadel.JumpSpot27");
        navBuilder.removeEdgesBetween("CTF-Citadel.PathNode36", "CTF-Citadel.JumpSpot5");
        navBuilder.removeEdgesBetween("CTF-Citadel.PathNode75", "CTF-Citadel.JumpSpot26");
        navBuilder.removeEdgesBetween("CTF-Citadel.PathNode64", "CTF-Citadel.jumpspot9");
        navBuilder.removeEdgesBetween("CTF-Citadel.InventorySpot179", "CTF-Citadel.InventorySpot193");
        navBuilder.removeEdge("CTF-Citadel.PathNode26", "CTF-Citadel.PathNode23");
	}
	
	private static void tweakCTFBP2Concentrate(NavigationGraphBuilder navBuilder) {
        navBuilder.removeEdgesBetween("CTF-BP2-Concentrate.InventorySpot59", "CTF-BP2-Concentrate.PathNode81");
        navBuilder.removeEdgesBetween("CTF-BP2-Concentrate.xRedFlagBase1", "CTF-BP2-Concentrate.PathNode81");
        navBuilder.removeEdgesBetween("CTF-BP2-Concentrate.xRedFlagBase1", "CTF-BP2-Concentrate.PathNode74");
        navBuilder.removeEdgesBetween("CTF-BP2-Concentrate.AssaultPath12", "CTF-BP2-Concentrate.PathNode81");
        navBuilder.removeEdgesBetween("CTF-BP2-Concentrate.AssaultPath12", "CTF-BP2-Concentrate.PathNode74");
        navBuilder.removeEdgesBetween("CTF-BP2-Concentrate.JumpSpot2", "CTF-BP2-Concentrate.PathNode74");
        navBuilder.removeEdgesBetween("CTF-BP2-Concentrate.JumpSpot2", "CTF-BP2-Concentrate.PathNode81");
        navBuilder.removeEdgesBetween("CTF-BP2-Concentrate.JumpSpot11", "CTF-BP2-Concentrate.JumpSpot13");
        navBuilder.removeEdgesBetween("CTF-BP2-Concentrate.JumpSpot11", "CTF-BP2-Concentrate.PathNode40");
        navBuilder.removeEdgesBetween("CTF-BP2-Concentrate.JumpSpot11", "CTF-BP2-Concentrate.JumpSpot14");
        navBuilder.removeEdgesBetween("CTF-BP2-Concentrate.JumpSpot11", "CTF-BP2-Concentrate.PathNode31");
        navBuilder.removeEdgesBetween("CTF-BP2-Concentrate.JumpSpot14", "CTF-BP2-Concentrate.PathNode75");
        navBuilder.removeEdgesBetween("CTF-BP2-Concentrate.JumpSpot13", "CTF-BP2-Concentrate.PathNode75");
        navBuilder.removeEdgesBetween("CTF-BP2-Concentrate.InventorySpot55", "CTF-BP2-Concentrate.PathNode44");
        navBuilder.removeEdgesBetween("CTF-BP2-Concentrate.xBlueFlagBase0", "CTF-BP2-Concentrate.PathNode44");
        navBuilder.removeEdgesBetween("CTF-BP2-Concentrate.xBlueFlagBase0", "CTF-BP2-Concentrate.PathNode0");
        navBuilder.removeEdgesBetween("CTF-BP2-Concentrate.JumpSpot3", "CTF-BP2-Concentrate.PathNode0");
        navBuilder.removeEdgesBetween("CTF-BP2-Concentrate.JumpSpot3", "CTF-BP2-Concentrate.PathNode44");
        navBuilder.removeEdgesBetween("CTF-BP2-Concentrate.JumpSpot4", "CTF-BP2-Concentrate.JumpSpot6");
        navBuilder.removeEdgesBetween("CTF-BP2-Concentrate.JumpSpot4", "CTF-BP2-Concentrate.JumpSpot5");
        navBuilder.removeEdgesBetween("CTF-BP2-Concentrate.JumpSpot4", "CTF-BP2-Concentrate.PathNode18");
        navBuilder.removeEdgesBetween("CTF-BP2-Concentrate.JumpSpot4", "CTF-BP2-Concentrate.PathNode30");
        navBuilder.removeEdgesBetween("CTF-BP2-Concentrate.PathNode39", "CTF-BP2-Concentrate.JumpSpot6");
        navBuilder.removeEdgesBetween("CTF-BP2-Concentrate.PathNode39", "CTF-BP2-Concentrate.JumpSpot5");
        navBuilder.removeEdgesBetween("CTF-BP2-Concentrate.AssaultPath5", "CTF-BP2-Concentrate.PathNode44");
        navBuilder.removeEdgesBetween("CTF-BP2-Concentrate.AssaultPath5", "CTF-BP2-Concentrate.PathNode0");
	}

	
	// ======================
	// CTF-Loftfaith
	// ======================
	
	private static void tweakCTFLostfaith(NavigationGraphBuilder navBuilder) {
	}
	
	// ======================
	// CTF-January
	// ======================

	private static void tweakCTFJanuary(NavigationGraphBuilder navBuilder) {
	}

	// ======================
	// CTF-DoubleDammage
	// ======================
	
	private static void tweakCTFDoubleDammage(NavigationGraphBuilder navBuilder) {		
	}
	
}
