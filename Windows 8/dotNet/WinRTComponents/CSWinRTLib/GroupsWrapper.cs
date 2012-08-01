using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace CSWinRTLib
{
    // TODO: 1 - CTWinRTLib is marked as a WinMD Output Type in Properties.
    // TODO: 2 - Class1 defines properties/methods/structs that follow the simple WinRT component rules.
    public sealed class GroupsWrapper
    {
        public string AppName 
        {
            get
            {
                return "My Hybrid App";
            }
        }

        public static IList<Group> GetGroups()
        {
            List<Group> groups = new List<Group>();

            groups.Add(new Group() { Name = "Group 1", Count = 4 });
            groups.Add(new Group() { Name = "My Hybrid Group 2", Count = 5 });

            return groups;
        }

        public static IAsyncOperation<IList<Group>> GetGroupsAsync(string id)
        {
            return Task.Run<IList<Group>>(() =>
            {
                Task.Delay(5000);

                List<Group> groups = new List<Group>();

                groups.Add(new Group() { Name = "My Hybrid Group 1", Count = 4 });
                groups.Add(new Group() { Name = "My Hybrid Group 2", Count = 5 });

                return groups;
 
            }).AsAsyncOperation();
        }
    }

    public struct Group
    {
        public string Name;
        public int Count;
    }
}
