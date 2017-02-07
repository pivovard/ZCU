import java.net.*;
import javax.swing.JOptionPane;

import java.awt.Window;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

import javax.swing.*;


class MyDialog
{
	private Timer timer;
	private JDialog dialog;
	private JOptionPane op;

	public MyDialog(String title, String msg, int time) {
		op = new JOptionPane(msg, JOptionPane.PLAIN_MESSAGE, JOptionPane.DEFAULT_OPTION, null, new Object[]{}, null);
		
		dialog = new JDialog();
		dialog.setTitle(title);
		dialog.setModal(false);

		dialog.setContentPane(op);

		dialog.setDefaultCloseOperation(JDialog.DISPOSE_ON_CLOSE);
		dialog.pack();

		if(time < 1) time = 1;
		//create timer to dispose of dialog after 5 seconds
		this.timer = new Timer(time * 1000, new AbstractAction() {
		    @Override
		    public void actionPerformed(ActionEvent ae) {
		        dialog.dispose();
		    }
		});
		timer.setRepeats(false);//the timer should only go off once

		//start timer to close JDialog as dialog modal we must start the timer before its visible
		timer.start();

		dialog.setVisible(true);
	}
	
	public void close(){
		this.timer.stop();
		this.op.setVisible(false);
		this.op.invalidate();
		this.dialog.setVisible(false);
		this.dialog.dispose();
	}
}
