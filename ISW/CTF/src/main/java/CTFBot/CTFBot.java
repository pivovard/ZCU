package CTFBot;

import CTFBot.MapView.CoverMapView;
import CTFBot.MapView.MapView;
import CTFBot.TeamComm.*;
import cz.cuni.amis.pathfinding.alg.astar.AStarResult;
import cz.cuni.amis.pogamut.base.agent.module.comm.PogamutJVMComm;
import cz.cuni.amis.pogamut.base.agent.navigation.IPathExecutorState;
import cz.cuni.amis.pogamut.base.agent.navigation.impl.PrecomputedPathFuture;
import cz.cuni.amis.pogamut.base.communication.worldview.listener.annotation.ObjectClassEventListener;
import cz.cuni.amis.pogamut.base.communication.worldview.listener.annotation.EventListener;
import cz.cuni.amis.pogamut.base.communication.worldview.object.IWorldObject;
import cz.cuni.amis.pogamut.base.communication.worldview.object.event.WorldObjectUpdatedEvent;
import cz.cuni.amis.pogamut.base.utils.math.DistanceUtils;
import cz.cuni.amis.pogamut.base3d.worldview.object.ILocated;
import cz.cuni.amis.pogamut.base3d.worldview.object.Location;
import cz.cuni.amis.pogamut.base3d.worldview.object.Rotation;
import cz.cuni.amis.pogamut.base3d.worldview.object.Velocity;
import cz.cuni.amis.pogamut.unreal.communication.messages.UnrealId;
import cz.cuni.amis.pogamut.ut2004.agent.module.sensor.AgentInfo;
import cz.cuni.amis.pogamut.ut2004.agent.module.utils.TabooSet;
import cz.cuni.amis.pogamut.ut2004.agent.module.utils.UT2004Skins;
import cz.cuni.amis.pogamut.ut2004.agent.navigation.navmesh.node.NavMeshPolygon;
import cz.cuni.amis.pogamut.ut2004.bot.impl.UT2004Bot;
import cz.cuni.amis.pogamut.ut2004.bot.impl.UT2004BotModuleController;
import cz.cuni.amis.pogamut.ut2004.communication.messages.ItemType;
import cz.cuni.amis.pogamut.ut2004.communication.messages.UT2004ItemType;
import cz.cuni.amis.pogamut.ut2004.communication.messages.gbcommands.Initialize;
import cz.cuni.amis.pogamut.ut2004.communication.messages.gbinfomessages.*;
import cz.cuni.amis.pogamut.ut2004.teamcomm.bot.UT2004BotTCController;
import cz.cuni.amis.pogamut.ut2004.teamcomm.server.UT2004TCServer;
import cz.cuni.amis.pogamut.ut2004.utils.UT2004BotRunner;
import cz.cuni.amis.utils.Cooldown;
import cz.cuni.amis.utils.exception.PogamutException;
import cz.cuni.amis.utils.flag.FlagListener;


import java.util.*;
import java.util.logging.Level;
import java.util.stream.Collectors;

/**
 * Created by pivov on 01-May-19.
 */
public class CTFBot extends UT2004BotTCController<UT2004Bot> {

    public static int BOTS_TO_START = 5;
    public static int TEAM = -1;
    public static int SKILL = 6;
    public static final boolean LOAD_LEVEL_GEOMETRY = false;
    public static final boolean RELOAD_NAVMESH = true;

    public boolean initialized = false;

    private TeamComm<CTFBot> comm;

    Map<UnrealId, Player> enemies = new HashMap<UnrealId, Player>();

    TabooSet<Item> tabooItems;
    Item targetItem;

    //region INIT
    @Override
    public void prepareBot(UT2004Bot bot) {
        //communication
        comm = new TeamComm<CTFBot>(this);

        // DEFINE WEAPON PREFERENCES
        initWeaponPreferences();

        tabooItems = new TabooSet<Item>(bot);

        // listeners
        nmNav.getPathExecutor().getState().addListener(
                new FlagListener<IPathExecutorState>() {
                    @Override
                    public void flagChanged(IPathExecutorState changedValue) {
                        switch (changedValue.getState()) {
                            case STUCK:
                                if (targetItem != null) {
                                    tabooItems.add(targetItem, 10);
                                }
                                nmNav.stopNavigation();
                                collectWeapons();
                                break;
                            case STOPPED:
                                antiStuck = 0;
                                break;
                            case TARGET_REACHED:
                                antiStuck = 0;
                                break;
                            case PATH_COMPUTATION_FAILED:
                                if (targetItem != null) {
                                    tabooItems.add(targetItem, 10);
                                }
                                break;
                        }
                    }
                });
    }

    private void initWeaponPreferences() {
        weaponPrefs.addGeneralPref(UT2004ItemType.MINIGUN, true);
        weaponPrefs.addGeneralPref(UT2004ItemType.LINK_GUN, true);
        weaponPrefs.addGeneralPref(UT2004ItemType.LIGHTNING_GUN, true);
        weaponPrefs.addGeneralPref(UT2004ItemType.SHOCK_RIFLE, true);
        weaponPrefs.addGeneralPref(UT2004ItemType.ROCKET_LAUNCHER, true);
        weaponPrefs.addGeneralPref(UT2004ItemType.FLAK_CANNON, false);
        weaponPrefs.addGeneralPref(UT2004ItemType.ASSAULT_RIFLE, false);
        weaponPrefs.addGeneralPref(UT2004ItemType.ASSAULT_RIFLE, true);
        weaponPrefs.addGeneralPref(UT2004ItemType.BIO_RIFLE, true);
        weaponPrefs.addGeneralPref(UT2004ItemType.SHIELD_GUN, false);

        weaponPrefs.newPrefsRange(400).add(UT2004ItemType.FLAK_CANNON, true)
                .add(UT2004ItemType.MINIGUN, true)
                .add(UT2004ItemType.LIGHTNING_GUN, true)
                .add(UT2004ItemType.LINK_GUN, true)
                .add(UT2004ItemType.ROCKET_LAUNCHER, true);

        weaponPrefs.newPrefsRange(1000).add(UT2004ItemType.LIGHTNING_GUN, true)
                .add(UT2004ItemType.SHOCK_RIFLE, true)
                .add(UT2004ItemType.LINK_GUN, true)
                .add(UT2004ItemType.ROCKET_LAUNCHER, true)
                .add(UT2004ItemType.MINIGUN, true);
    }

    @Override
    protected void initializeModules(UT2004Bot bot) {
        super.initializeModules(bot);
        levelGeometryModule.setAutoLoad(LOAD_LEVEL_GEOMETRY);
    }

    @Override
    public void mapInfoObtained() {
        super.mapInfoObtained();
        MapTweaks.tweak(navBuilder);
        navMeshModule.setReloadNavMesh(RELOAD_NAVMESH);
    }

    private static int botsCount = 0;
    @Override
    public Initialize getInitializeCommand() {
        botsCount++;

        //if team is set -> make one team
        if(TEAM > -1){
            return new Initialize().setName("CTFBot"+botsCount).setDesiredSkill(SKILL).setSkin(UT2004Skins.getRandomSkin()).setTeam(TEAM);
        }

        //else two teams
        if(botsCount % 2 == 0){
            return new Initialize().setName("CTFBot"+botsCount).setDesiredSkill(SKILL).setSkin(UT2004Skins.getRandomSkin()).setTeam(AgentInfo.TEAM_RED);
        }
        else{
            return new Initialize().setName("CTFBot"+botsCount).setDesiredSkill(SKILL).setSkin(UT2004Skins.getRandomSkin()).setTeam(AgentInfo.TEAM_BLUE);
        }
    }

    @Override
    public void botInitialized(GameInfo gameInfo, ConfigChange currentConfig, InitedMessage init) {
        //this.gameInfo = gameInfo;
        bot.getLogger().getCategory("Yylex").setLevel(Level.OFF);
    }

    @Override
    public void botFirstSpawn(GameInfo gameInfo, ConfigChange currentConfig, InitedMessage init, Self self) {
        PogamutJVMComm.getInstance().registerAgent(bot, self.getTeam());
    }

    @Override
    public void botShutdown() {
        PogamutJVMComm.getInstance().unregisterAgent(bot);
    }

    //endregion

    //region LISTENERS

    //see player
    @ObjectClassEventListener(eventClass=WorldObjectUpdatedEvent.class, objectClass=Player.class)
    public void playerUpdated(WorldObjectUpdatedEvent<Player> event) {
        if(!initialized) return;

        Player player = event.getObject();
        if (player != null && !player.isSpectator() && player.isVisible() && players.isEnemy(player)) {
            enemies.put(player.getId(), player);
            comm.sendPlayer(player);

            if(isICarryingFlag()) return;
            fight();
        }
    }

    //player killed
    @EventListener(eventClass = PlayerKilled.class)
    public void playerKilled(PlayerKilled event) {
        if(!initialized) return;

        UnrealId botDiedId = event.getId();
        if (botDiedId == null) return;

        enemies.remove(botDiedId);
    }

    //get damage
    @EventListener(eventClass = BotDamaged.class)
    public void botDamaged(BotDamaged event) {
        if(!initialized) return;

        int damage = event.getDamage();

        if (event.getInstigator() != null) {
            UnrealId id = event.getInstigator();
            Player player = (Player) world.get(id);
            if(player != null && players.isEnemy(player)){
                enemies.put(player.getId(), player);
                comm.sendPlayer(player);

                if(isICarryingFlag()) return;
                move.turnTo(player);
                fight();
            }
        }
    }

    //noise
    @EventListener(eventClass = HearNoise.class)
    public void hearNoise(HearNoise event) {
        if(!initialized) return;

        double noiseDistance = event.getDistance();   // 100 ~ 1 meter
        Rotation faceRotation = event.getRotation();  // rotate bot to this if you want to face the location of the noise
        if(!players.canSeeEnemies()) return;
        //fight();
    }

    //dodge incoming projectiles
    @ObjectClassEventListener(objectClass = IncomingProjectile.class, eventClass = WorldObjectUpdatedEvent.class)
    protected void incomingProjectile(WorldObjectUpdatedEvent<IncomingProjectile> event) {
        if(!initialized) return;

        Location projectileLocation = event.getObject().getLocation();
        Velocity projectileVelocity = event.getObject().getVelocity();

        if (event.getObject().getLocation().getDistance(info.getLocation()) < 750) {
            dodge();
        }
    }

    //item picked
    @EventListener(eventClass = ItemPickedUp.class)
    public void itemPickedUp(ItemPickedUp event) {
        if(!initialized) return;

        ItemType itemType = event.getType();
        ItemType.Group itemGroup = itemType.getGroup();
        ItemType.Category itemCategory = itemType.getCategory();

        comm.sendItem(event);
    }

    //item changed state
    @ObjectClassEventListener(objectClass = Item.class, eventClass = WorldObjectUpdatedEvent.class)
    public void itemUpdated(WorldObjectUpdatedEvent<Item> event) {
        if(!initialized) return;

        if (info.getLocation() == null) return;
        Item item = event.getObject();
    }

    //flag status changed
    @ObjectClassEventListener(objectClass = FlagInfo.class, eventClass = WorldObjectUpdatedEvent.class)
    public void flagInfoUpdated(WorldObjectUpdatedEvent<FlagInfo> event) {
        if(!initialized) return;

        comm.sendFlag(event.getObject());

        if(isICarryingFlag()) return;
        if(isFighting) return;
        getFlag();
        if(isNavigating()) return;
    }

    @EventListener(eventClass=TCFlagUpdate.class)
    public void flagUpdate(TCFlagUpdate msg) {
        if(!initialized) return;

        IWorldObject obj = getWorldView().get(msg.id);
        if (obj == null || !(obj instanceof FlagInfo)) return;

        FlagInfo flag = (FlagInfo)obj;
        getWorldView().notify(flag);

        if(ctf. isOurTeamCarryingEnemyFlag() && flag.getId() == ctf.getEnemyFlag().getId()){
            flagCarrier = players.getPlayer(flag.getHolder());
        }

        if(isFighting) return;
        getFlag();
        if(isNavigating()) return;
    }

    @EventListener(eventClass=TCItemUpdate.class)
    public void itemUpdate(TCItemUpdate msg) {
        if(!initialized) return;

        IWorldObject obj = getWorldView().get(msg.id);
        if (obj == null || !(obj instanceof Item) || msg == null) return;

        Item item = (Item)obj;
        if(msg.spawned){
            tabooItems.add(item, 10);
        }
        else{
            tabooItems.add(item, items.getItemRespawnTime(item));
        }
    }

    @EventListener(eventClass=TCPlayerUpdate.class)
    public void playerUpdate(TCPlayerUpdate msg) {
        if(!initialized) return;

        IWorldObject obj = getWorldView().get(msg.id);
        if (obj == null || !(obj instanceof Player)) return;

        Player player = (Player)obj;
        enemies.put(player.getId(), player);
    }

    //endregion

    @Override
    public void beforeFirstLogic() {
        if(!initialized) initialized = true;
    }

    @Override
    public void logic() throws PogamutException {
        //body.getCommunication().sendGlobalTextMessage("");

        if (isICarryingFlag()) return;

        //fight schema
        //if (fight()) return;
        if (info.isShooting() && !players.canSeeEnemies()) shoot.stopShooting();

        if (!ctf.isOurFlagHome()) {
            getOurFlag();
            if (isNavigating()) return;
        }

        //collect schema
        if (bot.getSelf().getHealth() < 50) {
            collectMedpack();
            if (isNavigating()) return;
        }
        if (isNavigating()) return;

        if (weaponry.getWeapons().size() < 3 || getMaxAmmo() < 50) {
            collectWeapons();
            if (isNavigating()) return;
        }
        if (isNavigating()) return;

        //flag schema
        getFlag();
        if (isNavigating()) return;
        collectWeapons();
    }

    private ILocated prevTarget = null;
    private int antiStuck = 0;

    private boolean isNavigating(){
        if(nmNav.isNavigating()){
            if(antiStuck++ > 15) {
                antiStuck = 0;
                move.doubleJump();
            }
            if (ctf.isBotCarryingEnemyFlag()) return true;
            itemNearby();
            return true;
        }else{
            if(prevTarget != null){
                nmNav.navigate(prevTarget);
                prevTarget = null;
                return true;
            }
            return false;
        }
    }

    private void itemNearby(){
        ILocated prevT = nmNav.getCurrentTarget();
        Item newT = items.getNearestItem();
        if(prevT == null || newT == null || !items.isPickupSpawned(newT) || !items.isPickable(newT)) return;
        double dist = newT.getLocation().getDistance(bot.getLocation());
        if(dist < prevT.getLocation().getDistance(bot.getLocation()) && dist < 300){
            prevTarget = prevT;
            nmNav.navigate(newT);
        }
    }

    private boolean isFighting = false;

    private boolean fight(){
        if(bot.getSelf().getHealth() < 20) {
            collectMedpack();
            isFighting = false;
            return false;
        }

        if(getMaxAmmo() < 10){
            collectWeapons();
            isFighting = false;
            return false;
        }

        if (players.canSeeEnemies()) {
            enemies.putAll(players.getVisibleEnemies());

            Player enemy = players.getNearestVisibleEnemy();
            shoot(enemy);

            if(enemy != null && bot.getLocation().getDistance(enemy.getLocation()) < 300){
                dodge();
            }
            else{
                //nmNav.navigate(enemy);
            }
            isFighting = true;
            return true;
        }
        else{
            if (info.isShooting() && !players.canSeeEnemies()) shoot.stopShooting();if (info.isShooting() && !players.canSeeEnemies()) shoot.stopShooting();

            if(enemies.size() > 0 || bot.getSelf().getHealth() > 50 || getMaxAmmo() > 50) {
                Player enemy = getNearestEnemy();
                if(enemy != null && bot.getLocation().getDistance(enemy.getLocation()) < 500){
                    //nmNav.navigate(enemy);
                    //return true;
                }
            }

            isFighting = false;
            return false;
        }
    }

    Cooldown lightCD = new Cooldown(2000);
    private void shoot(Player enemy){
        if(enemy == null) return;

        //shoot.shoot(weaponPrefs, enemy);
        if(weaponry.hasWeapon(UT2004ItemType.LIGHTNING_GUN)){
            double diff = 0;
            if (lightCD.isCool()) {
                // ROTATE TO TARGET BEFORE LIGHTNIN_GUN FIRES
                Location vecToTarget = enemy.getLocation().sub(info.getLocation()).getNormalized();
                Location rotationVector = info.getRotation().toLocation().getNormalized();
                diff = (vecToTarget.sub(rotationVector)).getLength();
                move.turnTo(enemy);
                if (diff > 0.6) return;
            }
            if (lightCD.tryUse()) {
                shoot.shoot(weaponPrefs, enemy);
            } else {
                // COOLDOWN ACTIVE                                          // DO NOT USE LIGHTNING_GUN
                shoot.shoot(weaponPrefs, enemy, UT2004ItemType.LIGHTNING_GUN);
            }
        }
        else if(weaponry.hasWeapon(UT2004ItemType.SHOCK_RIFLE)){
            double diff = 0;
            if (lightCD.isCool()) {
                // ROTATE TO TARGET BEFORE LIGHTNIN_GUN FIRES
                Location vecToTarget = enemy.getLocation().sub(info.getLocation()).getNormalized();
                Location rotationVector = info.getRotation().toLocation().getNormalized();
                diff = (vecToTarget.sub(rotationVector)).getLength();
                move.turnTo(enemy);
                if (diff > 0.6) return;
            }
            if (lightCD.tryUse()) {
                shoot.shoot(weaponPrefs, enemy);
            } else {
                // COOLDOWN ACTIVE                                          // DO NOT USE LIGHTNING_GUN
                shoot.shoot(weaponPrefs, enemy, UT2004ItemType.SHOCK_RIFLE);
            }
        }
        else{
            move.turnTo(enemy);
            shoot.shoot(weaponPrefs, enemy);
        }
    }

    private Player getNearestEnemy(){
        if(enemies.size() == 0) return null;
        Player enemy = null;
        Location pos = bot.getLocation();
        for(Player pl : enemies.values()){
            if(enemy == null || pl.getLocation().getDistance(pos) < enemy.getLocation().getDistance(pos)){
                enemy = pl;
            }
        }
        return enemy;
    }

    private void dodge(){
        //body.getCommunication().sendGlobalTextMessage("Dodging");
        Location dodgeThisLocation = info.getLocation().add(info.getRotation().toLocation());
        if (random.nextDouble() < 0.5) {
            move.dodgeLeft(dodgeThisLocation, true);
        } else {
            move.dodgeRight(dodgeThisLocation, true);
        }
    }

    private void collectMedpack(){
        targetItem = DistanceUtils.getNearest(
                items.getSpawnedItems(ItemType.Category.HEALTH).values().stream().filter(
                        (item) -> {
                            return items.isPickable(item) && !tabooItems.isTaboo(item);
                        }
                ).collect(Collectors.toList()),
                info.getLocation());

        if (targetItem != null) {
            comm.sendItem(targetItem);
            nmNav.navigate(targetItem);
        }
    }

    private void collectWeapons(){
        targetItem = DistanceUtils.getNearest(
            items.getSpawnedItems().values().stream().filter(
                    (item) -> {
                        return items.isPickable(item) && !tabooItems.isTaboo(item);
                        }
            ).collect(Collectors.toList()),
            info.getLocation(),
            new DistanceUtils.IGetDistance<Item>() {
                @Override
                public double getDistance(Item object, ILocated target) {
                    double multi = 1;
                    if      (object.getType() == UT2004ItemType.LIGHTNING_GUN) multi = 0.1;
                    else if (object.getType() == UT2004ItemType.SHOCK_RIFLE) multi = 0.1;
                    else if (object.getType() == UT2004ItemType.LINK_GUN) multi = 0.1;
                    else if (object.getType() == UT2004ItemType.MINI_HEALTH_PACK) multi = 0.1;
                    else if (object.getType().getCategory() == ItemType.Category.SHIELD) multi = 0.2;
                    else if (object.getType().getCategory() == ItemType.Category.WEAPON) multi = 0.4;
                    return multi * navMeshModule.getAStarPathPlanner().getDistance(object, target);
                }
            });

        if (targetItem != null) {
            comm.sendItem(targetItem);
            nmNav.navigate(targetItem);
        }
    }

    private int getMaxAmmo(){
        int ammo = 0;
        for (int a : weaponry.getAmmos().values()){
            ammo += a;
        }
        return ammo;
    }

    private void getFlag(){
        if (isICarryingFlag()) return;

        if(ctf.isOurTeamCarryingEnemyFlag() && ctf.isOurFlagHome()){
            protectFlagCarrier();
            return;
        }
        if(!ctf.isOurFlagHome()){
            getOurFlag();
            return;
        }
        if(!ctf.isEnemyFlagHome()){
            getEnemyFlag();
            return;
        }
        goToEnemyBase();
    }

    private boolean isICarryingFlag(){
        if (ctf.isBotCarryingEnemyFlag()) {
            goToOurBase();
            if (players.canSeeEnemies()) shoot(players.getNearestVisibleEnemy());
            return true;
        }
        return false;
    }

    private void goToEnemyBase(){
        if(nmNav.isNavigating() && nmNav.getCurrentTarget().getLocation() == ctf.getEnemyBase().getLocation()) return;
        PrecomputedPathFuture<ILocated> path = getAStarPathCover(ctf.getEnemyBase());
        //nmNav.navigate((IPathFuture)path);
        nmNav.navigate(ctf.getEnemyBase());
    }

    private void goToOurBase(){
        if(nmNav.isNavigating() && nmNav.getCurrentTarget().getLocation() == ctf.getOurBase().getLocation()) return;
        PrecomputedPathFuture<ILocated> path = getAStarPathCover(ctf.getOurBase());
        //nmNav.navigate((IPathFuture)path);
        nmNav.navigate(ctf.getOurBase());
    }


    private Player flagCarrier;
    private void protectFlagCarrier(){
        if(!isFighting && flagCarrier != null) nmNav.navigate(flagCarrier);
    }

    private void getOurFlag(){
        if(nmNav.isNavigating() && nmNav.getCurrentTarget().getLocation() == ctf.getEnemyFlag().getLocation()) return;
        NavPoint p = info.getNearestNavPoint(ctf.getOurFlag().getLocation());
        if(p == null){
            goToEnemyBase();
            return;
        }
        PrecomputedPathFuture<ILocated> path = getAStarPath(p);
        //nmNav.navigate((IPathFuture)path);
        nmNav.navigate(ctf.getOurFlag().getLocation());
    }

    private void getEnemyFlag(){
        if(nmNav.isNavigating() && nmNav.getCurrentTarget().getLocation() == ctf.getEnemyFlag().getLocation()) return;
        NavPoint p = info.getNearestNavPoint(ctf.getEnemyFlag().getLocation());
        if(p == null){
            goToEnemyBase();
            return;
        }
        PrecomputedPathFuture<ILocated> path = getAStarPath(p);
        //nmNav.navigate((IPathFuture)path);
        nmNav.navigate(ctf.getEnemyFlag().getLocation());
    }


    private PrecomputedPathFuture<ILocated> getAStarPath(NavPoint targetNavPoint) {
        NavPoint startNavPoint = info.getNearestNavPoint();
        AStarResult<NavPoint> result = aStar.findPath(startNavPoint, targetNavPoint, new MapView(players, visibility));
        if (result == null || !result.isSuccess()) return null;
        PrecomputedPathFuture path = new PrecomputedPathFuture(startNavPoint, targetNavPoint, result.getPath());
        return path;
    }

    private PrecomputedPathFuture<ILocated> getAStarPathCover(NavPoint targetNavPoint) {
        NavPoint startNavPoint = info.getNearestNavPoint();
        AStarResult<NavPoint> result = aStar.findPath(startNavPoint, targetNavPoint, new CoverMapView(players, visibility));
        if (result == null || !result.isSuccess()) return null;
        PrecomputedPathFuture path = new PrecomputedPathFuture(startNavPoint, targetNavPoint, result.getPath());
        return path;
    }

    private Location getNavMeshLocation(ILocated location) {
        if (location == null || location.getLocation() == null) return null;
        NavMeshPolygon nmPoly = navMeshModule.getDropGrounder().tryGround(location);
        if (nmPoly == null) return null;
        Location nmLoc = new Location(nmPoly.getShape().project(location.getLocation().asPoint3D()));
        return nmLoc;
    }



    public static void main(String args[]) throws PogamutException {
        UT2004TCServer tcServer = UT2004TCServer.startTCServer();

        if(args.length == 0) {
            new UT2004BotRunner(CTFBot.class, "CTFBot").setMain(true).setLogLevel(Level.INFO).startAgents(BOTS_TO_START);
        }
        else if(args.length == 5) {
            TEAM = Integer.parseInt(args[1]);
            SKILL = Integer.parseInt(args[2]);
            BOTS_TO_START = Integer.parseInt(args[3]);
            String server = args[4];

            new UT2004BotRunner(CTFBot.class, "CTFBot").setHost(server).setMain(true).setLogLevel(Level.WARNING).startAgents(BOTS_TO_START);
        }
        else {
            System.out.println("Wrong number of parameters!");
        }

    }
}