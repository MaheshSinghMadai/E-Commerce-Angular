import { Component, OnInit } from '@angular/core';
import { Product } from '../../shared/models/product';
import {MatCardModule} from '@angular/material/card';
import { MatDialog } from '@angular/material/dialog';
import { ShopService } from '../../core/services/shop.service';
import { ProductItemComponent } from "./product-item/product-item.component";
import { FilterDialogComponent } from './filter-dialog/filter-dialog.component';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatMenu, MatMenuTrigger } from '@angular/material/menu';
import { MatListOption, MatSelectionList, MatSelectionListChange } from '@angular/material/list';
import { ShopParams } from '../../shared/models/shopParams';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Pagination } from '../../shared/models/pagination';
import { FormsModule } from '@angular/forms';
import { EmptyStateComponent } from "../../shared/components/empty-state/empty-state.component";

@Component({
  selector: 'app-shop',
  standalone: true,
  imports: [
    MatCardModule,
    ProductItemComponent,
    MatButton,
    MatIcon,
    MatMenu,
    MatSelectionList,
    MatListOption,
    MatMenuTrigger,
    MatPaginator,
    FormsModule,
    EmptyStateComponent
],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss'
})
export class ShopComponent implements OnInit{

  products ?: Pagination<Product>;
  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'Price: Low-High', value: 'priceAsc' },
    { name: 'Price: High-Low', value: 'priceDsc' },
  ]

  pageSizeOptions = [ 5,10,15,20]
  shopParams = new ShopParams();

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
    this.getProducts();
  }

  getProducts(){
    this.shopService.getProducts(this.shopParams).subscribe({
      next: response => this.products = response,
      error: error => console.log(error),
    })
  }

  resetFilter(){
    this.shopParams = new ShopParams();
    this.getProducts();
  }

  onSearchChange(){
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  openFilterDialog(){
    const dialogRef = this.dialogService.open(FilterDialogComponent,
      {
        minWidth : '500px',
        data : {
          selectedBrands : this.shopParams.brands,
          selectedTypes : this.shopParams.types
        }
      }
    );

    dialogRef.afterClosed().subscribe({
      next: result => {
        if(result){
          console.log(result);
          this.shopParams.brands = result.selectedBrands;
          this.shopParams.types = result.selectedTypes;
          this.shopParams.pageNumber = 1;
          this.getProducts();
        }
      }
    })
  }

  onSortChange(event : MatSelectionListChange){
    const selectedOptions = event.options[0];
    if(selectedOptions){
      this.shopParams.sort = selectedOptions.value;
      this.shopParams.pageNumber = 1;
      this.getProducts();
    }
  }

  handlePageEvent(event: PageEvent){
    this.shopParams.pageNumber = event.pageIndex + 1;
    this.shopParams.pageSize = event.pageSize;
    this.getProducts();
  }
}