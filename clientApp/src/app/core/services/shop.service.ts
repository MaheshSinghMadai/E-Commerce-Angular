import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Product } from '../../shared/models/product';
import { Pagination } from '../../shared/models/pagination';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = 'https://localhost:7201';
  types: string[] = [];
  brands: string[] = [];
  constructor(private http: HttpClient) { }

  getProducts(brands?: string[], types?: string[]){
    let params = new HttpParams();

    if(brands && brands.length > 0){
      params = params.append('brands', brands.join(','));
    }

    if(types && types.length > 0){
      params = params.append('types', types.join(','));
    }

    params = params.append('pageSize', 20);

    return this.http.get<Pagination<Product>>(this.baseUrl + '/Product/GetProducts', {params});
  }

  getTypes(){
    if(this.types.length > 0) return;
    return this.http.get<string[]>(this.baseUrl + '/Product/GetTypes').subscribe({
      next: response => this.types = response,
    });
  }
  getBrands(){
    return this.http.get<string[]>(this.baseUrl + '/Product/GetBrands').subscribe({
      next: response => this.brands = response,
    });;
  }
}
