using System.Collections.Generic;

namespace YoungGuns.Shared
{
    public class CalcChangeset
    {
        /// <summary>
        /// User Id of the user pushing this changeset
        /// </summary>
        public string Owner { get; set; }
        
        /// <summary>
        /// Return Id for the return
        /// </summary>
        public string ReturnId { get; set; }
        /// <summary>
        /// Version of the return DAG on which changeset is based
        /// </summary>
        public int BaseVersion { get; set; }

        /// <summary>
        /// Changeset values to be updated (key: field ID, value: new value)
        /// </summary>
        public Dictionary<uint,float> NewValues { get; set; }
    }
}