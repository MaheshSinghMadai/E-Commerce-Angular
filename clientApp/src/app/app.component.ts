import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from "./layout/header/header.component";
import { HttpClient } from '@angular/common/http';
import { Product } from './shared/models/product';
import { ShopComponent } from "./features/shop/shop.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent, ShopComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
}