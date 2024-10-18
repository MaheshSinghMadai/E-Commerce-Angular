import { Component, inject, OnInit, output } from '@angular/core';
import { CheckoutService } from '../../../core/services/checkout.service';
import { CartService } from '../../../core/services/cart.service';
import { MatRadioModule } from '@angular/material/radio';
import { CurrencyPipe } from '@angular/common';
import { DeliveryEstimate } from '@stripe/stripe-js';
import { DeliveryMethod } from '../../../shared/models/deliveryMethod';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-checkout-delivery',
  standalone: true,
  imports: [
    MatRadioModule,
    CurrencyPipe
  ],
  templateUrl: './checkout-delivery.component.html',
  styleUrl: './checkout-delivery.component.scss'
})
export class CheckoutDeliveryComponent implements OnInit{
  
 checkoutService = inject(CheckoutService);
 cartService = inject(CartService);

  ngOnInit() {
    this.checkoutService.getDeliveryMethods().subscribe({
      next: methods => {
        if(this.cartService.cart()?.deliveryMethodId){
          const method = methods.find(x =>x.id === this.cartService.cart()?.deliveryMethodId)
          if(method){
            this.cartService.selectedDelivery.set(method);
          }
        }
      }
    });
  }

 updateDeliveryMethod(method: DeliveryMethod) {
    this.cartService.selectedDelivery.set(method);
    const cart = this.cartService.cart();
    if (cart) {
      cart.deliveryMethodId = method.id;
      this.cartService.setCart(cart);
    }
  }

}
