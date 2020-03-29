package CTFBot.TeamComm;

import CTFBot.CTFBot;
import cz.cuni.amis.pogamut.base.communication.worldview.listener.annotation.AnnotationListenerRegistrator;
import cz.cuni.amis.pogamut.base.utils.logging.LogCategory;
import cz.cuni.amis.pogamut.ut2004.communication.messages.gbinfomessages.*;
import cz.cuni.amis.pogamut.ut2004.teamcomm.bot.UT2004TCClient;

/**
 * Created by pivov on 13-May-19.
 */
public class TeamComm<BOTCTRL extends CTFBot> {

    private AnnotationListenerRegistrator listenerRegistrator;

    private BOTCTRL ctx;

    private UT2004TCClient comm;

    private LogCategory log;

    private boolean firstUpdate = true;

    public TeamComm(BOTCTRL ctx) {
        this.ctx = ctx;
        this.log = ctx.getBot().getLogger().getCategory("CTFCommObjectUpdates");
        this.comm = ctx.getTCClient();

        // INITIALIZE @EventListener SNOTATED LISTENERS
        listenerRegistrator = new AnnotationListenerRegistrator(this, ctx.getWorldView(), ctx.getBot().getLogger().getCategory("Listeners"));
        listenerRegistrator.addListeners();
    }

    public void sendMe() {
        comm.sendToTeamOthers(new TCPlayerUpdate(ctx.getInfo()));
    }

    public void sendItem(Item item) {
        comm.sendToTeamOthers(new TCItemUpdate(item));
    }

    public void sendItem(ItemPickedUp item) {
        comm.sendToTeamOthers(new TCItemUpdate(item));
    }

    public void sendPlayer(Player player) {
        comm.sendToTeamOthers(new TCPlayerUpdate(player));
    }

    public void sendFlag(FlagInfo flag) {
        comm.sendToTeamOthers(new TCFlagUpdate(flag));
    }

    public void sendFlags() {
        for (FlagInfo flag : ctx.getWorldView().getAllVisible(FlagInfo.class).values()) {
            comm.sendToTeamOthers(new TCFlagUpdate(flag));
        }
        if (ctx.getCTF().isBotCarryingEnemyFlag() && !ctx.getCTF().getEnemyFlag().isVisible()) {
            comm.sendToTeamOthers(new TCFlagUpdate(ctx.getCTF().getEnemyFlag()));
        }
    }
}
