import { Component, Input } from '@angular/core';
import { Product } from '../../../shared/models/product';
import { MatCardActions, MatCardContent, MatCardModule } from '@angular/material/card';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { MatButton } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-product-item',
  standalone: true,
  imports: [
    MatCardModule,
    MatCardContent,
    CurrencyPipe,
    MatCardActions,
    MatButton,
    MatIconModule
  ],
  templateUrl: './product-item.component.html',
  styleUrl: './product-item.component.scss'
})
export class ProductItemComponent {
 @Input() product?: Product;
}
