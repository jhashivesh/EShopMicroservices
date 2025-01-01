import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Pagination } from '../../shared/models/pagination';
import { Product } from '../../shared/models/product';

@Injectable({
  providedIn: 'root',
})
export class ShopService {
  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);

  getProducts() {
    console.log(
      'getProductsUrl::' + this.baseUrl + 'catalog-service/products/'
    );
    return this.http.get<Pagination<Product>>(
      this.baseUrl + 'catalog-service/products'
    );
  }

  getProduct(id: string) {
    return this.http.get<ProductResponse>(
      this.baseUrl + 'catalog-service/products/' + id
    );
  }
}

interface ProductResponse {
  product: Product;
}
