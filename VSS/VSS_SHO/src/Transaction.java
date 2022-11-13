/**
 * Created by pivov on 20-Jan-19.
 */
public class Transaction {

    /**
     * Transaction creation time
     */
    public double creationTime;

    /**
     * Creates transaction
     *
     * @param creationTime time when transaction was created
     */
    public Transaction(double creationTime) {
        this.creationTime = creationTime;
    }

    /**
     * Get creation time
     *
     * @return the creationTime
     */
    public double getCreationTime() {
        return creationTime;
    }

}
