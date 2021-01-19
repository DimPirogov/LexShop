using LexShop.Core.Contracts;
using LexShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LexShop.Services
{
    public class BasketService
    {
        IRepository<Product> productContext;
        IRepository<Basket> basketContext;
        public const string BasketSessionName = "eCommerceBasket";

        public BasketService(IRepository<Product> ProductContext, IRepository<Basket> BasketContext)
        {
            this.basketContext = BasketContext;
            this.productContext = ProductContext;
        }
        private Basket GetBasket(HttpContextBase httpContext, bool creatIfNull)
        {
            HttpCookie cookie = httpContext.Request.Cookies.Get(BasketSessionName); //reading cookie
            Basket basket = new Basket();
            if(cookie != null)
            {
                string basketId = cookie.Value;
                if (!string.IsNullOrEmpty(basketId))
                {
                    basket = basketContext.Find(basketId);
                }
                else
                {
                    if (creatIfNull)
                    {
                        basket = CreateNewBasket(httpContext);
                    }
                }
            }
            else
            {
                if (creatIfNull)
                {
                    basket = CreateNewBasket(httpContext);
                }
            }
            return basket;
        }
        private Basket CreateNewBasket(HttpContextBase httpContext)
        {
            Basket basket = new Basket();
            basketContext.Insert(basket);
            basketContext.Commit();

            HttpCookie cookie = new HttpCookie(BasketSessionName); //creating a cookie
            cookie.Value = basket.Id;   //adding a value an ID bound to products in DB
            cookie.Expires = DateTime.Now.AddDays(1);   //adding an expire date
            httpContext.Response.Cookies.Add(cookie);   //sending back a cookie to an user

            return basket;
        }
        public void AddToBasket(HttpContextBase httpContext, string productId)
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.ProductId == productId);

            if(item == null)    // in case item does not exists we start the count + 1
            {
                item = new BasketItem()
                {
                    BasketId = basket.Id,   //set basket ID to current basket ID
                    ProductId = productId,  //set product ID to current produsct ID
                    Quantity = 1            //set quantity to 1 as this is the first product of that type
                };
                basket.BasketItems.Add(item);
            }
            else
            {
                item.Quantity = item.Quantity + 1;  //if that item exists in the basket we increase the quantity
            }
            basketContext.Commit(); //commit the changes
        }
        public void RemoveFromBasket(HttpContextBase httpContext, string itemId)
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.Id == itemId);

            if( item != null)
            {
                basket.BasketItems.Remove(item);
                basketContext.Commit();
            }
        }
    }
}
