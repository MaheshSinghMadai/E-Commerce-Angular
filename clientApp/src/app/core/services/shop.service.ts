import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Product } from '../../shared/models/product';
import { Pagination } from '../../shared/models/pagination';
import { ShopParams } from '../../shared/models/shopParams';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ShopService {
  baseUrl = environment.apiUrl;
  types: string[] = [];
  brands: string[] = [];
  constructor(private http: HttpClient) {}

  getProducts(shopParams: ShopParams) {
    let params = new HttpParams();

    if (shopParams.brands.length > 0) {
      params = params.append('brands', shopParams.brands.join(','));
    }

    if (shopParams.types.length > 0) {
      params = params.append('types', shopParams.types.join(','));
    }

    if (shopParams.sort) {
      params = params.append('sort', shopParams.sort);
    }

    if (shopParams.search) {
      params = params.append('search', shopParams.search);
    }

    params = params.append('pageSize', shopParams.pageSize);
    params = params.append('pageIndex', shopParams.pageNumber);

    return this.http.get<Pagination<Product>>(
      this.baseUrl + 'Product/GetProducts',
      { params }
    );
  }
  getProduct(id: number) {
    return this.http
      .get<Product>(this.baseUrl + 'Product/GetProductById/?id=' + id);
  }

  getTypes() {
    if (this.types.length > 0) return;
    return this.http
      .get<string[]>(this.baseUrl + 'Product/GetTypes')
      .subscribe({
        next: (response) => (this.types = response),
      });
  }
  getBrands() {
    return this.http
      .get<string[]>(this.baseUrl + 'Product/GetBrands')
      .subscribe({
        next: (response) => (this.brands = response),
      });
  }
}
