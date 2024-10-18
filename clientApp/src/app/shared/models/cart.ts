import {nanoid} from 'nanoid';

export type CartType = {
    id: string;
    items: CartItem[];  
    deliveryMethodId?: number;
    paymentIntentId? : string;
    clientSecret?: string;
}

export type CartItem = {
    productId: number;
    productName: string;
    price: number;
    quantity: number;
    pictureUrl: string;
    brand: string;
    type: string;
}

export class Cart implements CartType  {
    deliveryMethodId?: number ;
    paymentIntentId?: string | undefined;
    clientSecret?: string | undefined;
    id = nanoid();
    items: CartItem[] = [];
}