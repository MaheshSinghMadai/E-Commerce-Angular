import { Component, OnInit } from '@angular/core';
import { Product } from '../../shared/models/product';
import {MatCardModule} from '@angular/material/card';
import { MatDialog } from '@angular/material/dialog';
import { ShopService } from '../../core/services/shop.service';
import { ProductItemComponent } from "./product-item/product-item.component";
import { FilterDialogComponent } from './filter-dialog/filter-dialog.component';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-shop',
  standalone: true,
  imports: [
    MatCardModule,
    ProductItemComponent,
    MatButton,
    MatIcon
],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss'
})
export class ShopComponent implements OnInit{
  title = 'Skinet Ecommerce Web App';
  products: Product[] = [];
  selectedBrands : string[] = [];
  selectedTypes : string[] = [];

  constructor(
    private shopService: ShopService,
    private dialogService: MatDialog
  ) { }
  ngOnInit() {
    this.initializeShop();
  }

  initializeShop(){
    this.shopService.getBrands();
    this.shopService.getTypes();
    this.shopService.getProducts().subscribe({
      next: response => this.products = response.data,
      error: error => console.log(error),
    })
  }

  openFilterDialog(){
    const dialogRef = this.dialogService.open(FilterDialogComponent,
      {
        minWidth : '500px',
        data : {
          selectedBrands : this.selectedBrands,
          selectedTypes : this.selectedTypes
        }
      }
    );

    dialogRef.afterClosed().subscribe({
      next: result => {
        if(result){
          console.log(result);
          this.selectedBrands = result.selectedBrands;
          this.selectedTypes = result.selectedTypes;
          this.shopService.getProducts(this.selectedBrands, this.selectedTypes).subscribe({
            next: response  => this.products = response.data,
            error: error => console.log(error),
          })
        }
      }
    })
  }
}