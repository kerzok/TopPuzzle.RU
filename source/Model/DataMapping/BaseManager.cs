namespace Toppuzzle.Model.DataMapping {
    public class BaseManager {
        public BaseManager(ISqlMapper sqlMapeer) {
            SqlMapper = sqlMapeer;
        }

        protected ISqlMapper SqlMapper { get; private set; }
    }
}