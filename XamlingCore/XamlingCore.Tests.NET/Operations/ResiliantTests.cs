using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XamlingCore.Portable.Model.Resiliency;
using XamlingCore.Portable.Model.Response;

namespace XamlingCore.Tests.NET.Operations
{
    [TestClass]
    public class ResiliantTests
    {

        [TestMethod]
        public async Task Resiliant_Fail_NotFound()
        {
            var result = await new XResiliant(retries: 1).Run(_immediateNotFOund);

            Assert.IsFalse(result);
            Assert.AreEqual(result.Retries, 0);

        }

        [TestMethod]
        public async Task Resiliant_Fail_NotFound_NonAllowedFailure()
        {
            var result = await new XResiliant(retries: 2, allowedResultsCodes:OperationResults.Success).Run(_immediateNotFOund);

            Assert.IsFalse(result);
            Assert.AreEqual(result.Retries, 2);

        }

        [TestMethod]
        public async Task Resiliant_Pass_Immediate()
        {
            var result = await new XResiliant(retries: 0).Run(_immediate);

            Assert.IsTrue(result);
            Assert.AreEqual(result.Retries,0);
           
        }
        [TestMethod]
        public async Task Resiliant_Fail_Exception()
        {
            var result = await new XResiliant(retries: 3).Run(_exception);

            Assert.IsFalse(result);
            Assert.AreEqual(result.Retries, 3);
            Assert.IsNotNull(result.Exception);
        }

        [TestMethod]
        public async Task Resiliant_Fail_TooManyRetries()
        {
            var result = await new XResiliant(retries:3).Run(_retryFail);

            Assert.IsFalse(result);
            Assert.AreEqual(result.Retries, 3);
        }

        [TestMethod]
        public async Task Resiliant_Pass_After_Retry()
        {
            var result = await new XResiliant().Run(_retry);

            Assert.IsTrue(result);
            Assert.AreEqual(result.Retries, 1);
        }

        //bool tests

        [TestMethod]
        public async Task Resiliant_Bool_Pass_Immediate()
        {
            var result = await new XResiliant(retries: 0).RunBool(_immediateB);

            Assert.IsTrue(result);
            Assert.AreEqual(result.Retries,0);
           
        }
        [TestMethod]
        public async Task Resiliant_Bool_Fail_Exception()
        {
            var result = await new XResiliant(retries: 3).RunBool(_exceptionB);

            Assert.IsFalse(result);
            Assert.AreEqual(result.Retries, 3);
            Assert.IsNotNull(result.Exception);
        }

        [TestMethod]
        public async Task Resiliant_Bool_Fail_TooManyRetries()
        {
            var result = await new XResiliant(retries: 3).RunBool(_retryFailB);

            Assert.IsFalse(result);
            Assert.AreEqual(result.Retries, 3);
        }

        [TestMethod]
        public async Task Resiliant_Bool_Pass_After_Retry()
        {
            var result = await new XResiliant().RunBool(_retryB);

            Assert.IsTrue(result);
            Assert.AreEqual(result.Retries, 1);
        }

        [TestMethod]
        public async Task Resiliant_Bool_Pass_After_Retry_Exception()
        {
            var result = await XResiliant.Exception.RunBool(_exception_alt_B);

            Assert.IsFalse(result);
            Assert.AreEqual(result.Retries, 1);
        }

        //Object tests

        [TestMethod]
        public async Task Resiliant_String_Pass_Immediate()
        {
            var result = await new XResiliant(retries: 0).Run(_immediateS);

            Assert.IsTrue(result);
            Assert.AreEqual(result.Retries, 0);

        }
        [TestMethod]
        public async Task Resiliant_String_Fail_Exception()
        {
            var result = await new XResiliant(retries: 3).Run(_exceptionS);

            Assert.IsFalse(result);
            Assert.AreEqual(result.Retries, 3);
            Assert.IsNotNull(result.Exception);
        }

        [TestMethod]
        public async Task Resiliant_String_Fail_TooManyRetries()
        {
            var result = await new XResiliant(retries: 3).Run(_retryFailS);

            Assert.IsFalse(result);
            Assert.AreEqual(result.Retries, 3);
        }

        [TestMethod]
        public async Task Resiliant_String_Pass_After_Retry()
        {
            var result = await new XResiliant().Run(_retryS);

            Assert.IsTrue(result);
            Assert.AreEqual(result.Retries, 1);
        }

        private int count = 0;

        async Task<XResult<bool>> _retry()
        {
            await Task.Delay(500);
            if (count == 0)
            {
                count++;
                return XResult<bool>.GetFailed("Could not do someting");
            }

            return new XResult<bool>(true);
        }

        async Task<XResult<bool>> _retryFail()
        {
            await Task.Delay(500);

            return XResult<bool>.GetFailed("Could not do someting");
        }

        async Task<XResult<bool>> _exception()
        {
            await Task.Delay(500);

            throw new InvalidOperationException("Something went wrong");
        }

        async Task<XResult<bool>> _immediate()
        {
            await Task.Delay(500);

            return new XResult<bool>(true);
        }

        async Task<XResult<bool>> _immediateNotFOund()
        {
            await Task.Delay(500);

            return XResult<bool>.GetNotFound();
        }

        //Bool bits
        private int countb = 0;
        async Task<bool> _retryB()
        {
            await Task.Delay(500);
            if (countb == 0)
            {
                count++;
                return false;
            }

            return true;
        }

        async Task<bool> _retryFailB()
        {
            await Task.Delay(500);

            return false;
        }

        async Task<bool> _exceptionB()
        {
            await Task.Delay(500);

            throw new InvalidOperationException("Something went wrong");
        }


        private int count_alt_b = 0;
        
        async Task<bool> _exception_alt_B()
        {
            await Task.Delay(500);
            if (count_alt_b == 0)
            {
                count_alt_b++;
                throw new InvalidOperationException();
            }

            return false;
        }

        async Task<bool> _immediateB()
        {
            await Task.Delay(500);

            return true;
        }

        //object bits
        private int counts = 0;
        async Task<string> _retryS()
        {
            await Task.Delay(500);
            if (counts == 0)
            {
                count++;
                return null;
            }

            return "Jordan";
        }

        async Task<string> _retryFailS()
        {
            await Task.Delay(500);

            return null;
        }

        async Task<string> _exceptionS()
        {
            await Task.Delay(500);

            throw new InvalidOperationException("Something went wrong");
        }

        async Task<string> _immediateS()
        {
            await Task.Delay(500);

            return "Jordan";
        }

         
    }
}
