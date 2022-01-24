using Api.VlsDomain;
using Api.VlsDomain.EntityModel;
using Api.VlsDomain.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace ApiMsTest
{
    [TestClass]
    public class EpcisObjectEventRepositoryTest
    {
        private EpcisObjectEventRepository? cut;


        [TestInitialize]
        public void Init()
        {
            DbSet<EpcisObjectEvent>? mockSet = GetMockDbSet();

            Mock<IVlsContext>? mockContext = new Mock<IVlsContext>();
            mockContext.Setup(o => o.EpcisObjectEvents).Returns(mockSet);

            cut = new EpcisObjectEventRepository(mockContext.Object);
        }

        [TestMethod]
        public void Get_ShouldReturnSingleEntry()
        {
            List<EpcisObjectEvent>? epcisObjectEvents  = cut?.GetFiftyEntries();
            Assert.IsNotNull(epcisObjectEvents);
            Assert.AreEqual(1, epcisObjectEvents.Count);
        }

        private static DbSet<EpcisObjectEvent> GetMockDbSet()
        {
            IQueryable<EpcisObjectEvent>? queryable = new List<EpcisObjectEvent>()
            {
                new EpcisObjectEvent() {Id = new System.Guid(), Action = "OBSERVE"}
            }.AsQueryable();

            Mock<DbSet<EpcisObjectEvent>>? dbSet = new Mock<DbSet<EpcisObjectEvent>>();
            dbSet.As<IQueryable<EpcisObjectEvent>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<EpcisObjectEvent>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<EpcisObjectEvent>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<EpcisObjectEvent>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            return dbSet.Object;
        }
    }
}
