using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace TaskFlow.Tests.ViewModelTests
{
    public class DeleteModelTests
    {
        List<DeleteHistoryList> list = new List<DeleteHistoryList>()
        {
            new DeleteHistoryList()
            {
                Id = 1,
                todo = 1,
                deleteTime = DateTime.Now
            },
            new DeleteHistoryList()
            {
                Id = 2,
                todo = 2,
                deleteTime = DateTime.Now
            },
            new DeleteHistoryList()
            {
                Id = 3,
                todo = 3,
                deleteTime = DateTime.Now
            }
        };

        //Mock<IDeleteModel<DeleteHistoryList>> deleteModel;
        DeleteModel deleteModel;

        public DeleteModelTests()
        {
            deleteModel = new DeleteModel(true);
            deleteModel.test_list = list;
        }

        [Fact]
        public void TestAddingDeletion()
        {
            TodoItem item = new TodoItem("exist");
            item.Id = 4;
            item.InTrash = true;
            deleteModel.Test_SetupDeleteTime(item);

            if (deleteModel.Test_GetData().Any(x => x.todo == item.Id))
                Assert.True(true);
            else
                Assert.False(true);
        }

        [Fact]
        public void TestAddingNull()
        {
            TodoItem item = new TodoItem("exist");
            item.Id = 3;
            item.InTrash = true;

            deleteModel.Test_SetupDeleteTime(item);
            int count = 0;
            foreach (var data in deleteModel.Test_GetData())
            {
                if(data.todo == item.Id)
                {
                    count++;
                }
            }

            if (count == 1)
                Assert.True(true);
            else
                Assert.False(true);
        }

        [Fact]
        public void TestRemoveEntry()
        {
            TodoItem item = new TodoItem("exist");
            item.Id = 4;
            item.InTrash = true;

            deleteModel.Test_SetupDeleteTime(item);

            deleteModel.Test_Delete(item);

            if (deleteModel.Test_GetData().Count < 4)
                Assert.True(true);
            else
                Assert.False(true);
        }

        [Fact]
        public void TestRemoveAutomatically()
        {
            DeleteHistoryList deleteItem = new DeleteHistoryList()
            {
                Id = 4,
                todo = 4,
                deleteTime = DateTime.Now - TimeSpan.FromDays(32)
            };

            deleteModel.Test_Insert(deleteItem);
            deleteModel.Test_AutoDelete();

            if (deleteModel.Test_GetData().Count < 4)
                Assert.True(true);
            else
                Assert.False(true);
        }
    }
}
