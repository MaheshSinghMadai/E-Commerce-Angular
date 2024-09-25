import { computed, inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Cart, CartItem } from '../../shared/models/cart';
import { Product } from '../../shared/models/product';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CartService {
  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  cart = signal<Cart | null>(null);

  itemCount = computed(() => {
    const cart = this.cart();
    return cart && cart.items ? cart.items.reduce((sum, item) => sum + item.quantity, 0) : 0;
  });

  totals = computed(() => {
    const cart = this.cart();
    if(!cart) return null;
    const subTotal = cart.items.reduce((sum, item) => sum + item.price * item.quantity, 0);
    const shipping = 0;
    const discount = 0;
    
    return {
      subTotal,
      shipping,
      discount,
      total: subTotal + shipping - discount
    };
  })

  getCart(id: string) {
    return this.http
      .get<Cart>(this.baseUrl + 'ShoppingCart/GetCartById?id=' + id)
      .pipe(
        map((cart) => {
          this.cart.set(cart);
          return cart;
        })
      );
  }

  setCart(cart: Cart) {
    return this.http
      .post<Cart>(this.baseUrl + 'ShoppingCart/UpdateCart', cart)
      .subscribe({
        next: (cart) => this.cart.set(cart),
      });
  }

  addItemToCart(item: CartItem | Product, quantity = 1) {
    const cart = this.cart() ?? this.createCart(); //check exisiting cart, if not exists - create new one
    if (this.isProduct(item)) {
      item = this.mapProductToCartItem(item);  //convert into cart item if product passed
    }

    cart.items = this.addOrUpdateItems(cart.items, item, quantity); // add or update items in cart
    this.setCart(cart); // update cart
  }

  removeItemFromCart(productId: number, quantity = 1){
    const cart = this.cart();
    if(!cart) return;

    //find index of the product in the cart
    const index = cart.items.findIndex(x => x.productId === productId); 
    // checks if product found in cart
    if(index !== -1){         

      console.log(cart.items[index].quantity, quantity);
      // if products quantity is greater than specified quantity                     
      if(cart.items[index].quantity > quantity){
        
        // remove product by specified quantity
        cart.items[index].quantity =cart.items[index].quantity - quantity      
      } else{  

        // else remove the product from cart
        cart.items.splice(index, 1);                 
      }

      if(cart.items.length === 0){
        this.deleteCart();
      } else{
        this.setCart(cart);
      }
    }
  }

  deleteCart() {
    this.http.delete(this.baseUrl + 'ShoppingCart/DeleteCart?id=' + this.cart()?.id).subscribe({
      next: () => {
        localStorage.removeItem('cart_id');
        this.cart.set(null);
      }
    })
  }

  private addOrUpdateItems(items: CartItem[], item: CartItem, quantity: number): CartItem[] {
    // Ensure items is always an array
    items = items ?? []; 
    const index = items.findIndex((i) => i.productId === item.productId); 
    
    if (index === -1) {
      item.quantity = quantity;
      items.push(item);
    } else {
      items[index].quantity += quantity; 
    }
    return items;
  }

  // map product object to CartItem object
  private mapProductToCartItem(item: Product): CartItem {
    return {
      productId: item.id,
      productName: item.name,
      price: item.price,
      quantity: 0,
      pictureUrl: item.pictureUrl,
      brand: item.brand,
      type: item.type,
    };
  }

  //type guard that checks whether the passed item is of type Product.
  private isProduct(item: CartItem | Product): item is Product {
    return (item as Product).id !== undefined;
  }

  //creates a new empty cart, store cartId in localStorage
  private createCart(): Cart {
    const cart = new Cart();
    localStorage.setItem('cart_id', cart.id);
    return cart;
  }
}
