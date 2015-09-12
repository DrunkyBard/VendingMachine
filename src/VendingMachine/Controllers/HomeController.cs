using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.Mvc;
using VendingMachineApp.Business;
using VendingMachineApp.Business.Exceptions;
using VendingMachineApp.Commands;
using VendingMachineApp.DataAccess.Queries;
using VendingMachineApp.Filters;
using VendingMachineApp.Models;

namespace VendingMachineApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly RefundCommandHandler _refundCommandHandler;
        private readonly BuyCommandHandler _buyCommandHandler;

        public HomeController(RefundCommandHandler refundCommandHandler, BuyCommandHandler buyCommandHandler)
        {
            Contract.Requires(refundCommandHandler != null);
            Contract.Requires(buyCommandHandler != null);

            _refundCommandHandler = refundCommandHandler;
            _buyCommandHandler = buyCommandHandler;
        }

        [HttpGet]
        [ExceptionFilter]
        public ActionResult Index()
        {
            var query = new ShowVendingMachineQuery();
            var model = query.Ask();

            return View(model);
        }

        [HttpPost]
        public JsonResult Refund(IEnumerable<CoinViewModel> deposit)
        {
            Contract.Requires(deposit != null);

            var depositCoins = deposit
                .Select(x => new Coin(x.ParValue, x.Count))
                .ToArray();
            var command = new RefundCommand(depositCoins);
            var @event = _refundCommandHandler.Execute(command);

            return Json(@event);
        }

        [HttpPost]
        [ExceptionFilter(
            typeof(InsufficientAmountForBuyingGoodsException), 
            typeof(VendingMachineDoesNotHaveCoinsForRefundException))]
        public JsonResult Buy(IEnumerable<CoinViewModel> deposit, Guid goodsIdentity)
        {
            Contract.Requires(deposit != null);
            Contract.Requires(goodsIdentity != default(Guid));

            var depositCoins = deposit
                .Select(x => new Coin(x.ParValue, x.Count))
                .ToArray();
            var goods = new GoodsIdentity(goodsIdentity);
            Contract.Assume(!goods.Equals(default(GoodsIdentity)));
            var command = new BuyCommand(depositCoins, goods);
            var @event = _buyCommandHandler.Execute(command);

            return Json(@event);
        }
    }
}