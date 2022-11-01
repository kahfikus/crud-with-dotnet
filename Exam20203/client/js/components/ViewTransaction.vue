<template>
    <div>
        <input type="text" v-model="filterByName" @change="getTransactions()" />
        <table class="table">
            <thead class="thead-dark">
                <tr>
                    <th scope="col">Transaction ID</th>
                    <th scope="col">User Name</th>
                    <th scope="col">Makanan Name</th>
                    <th scope="col">Quantity</th>
                    <th scope="col">Transaction Date</th>
                   
                </tr>
            </thead>
            <tbody>
                <tr v-for="(transaction,index) in transactionView" :key="index">
                    <td>{{transaction.transactionId}}</td>
                    <td>{{transaction.userName}}</td>
                    <td>{{transaction.makananName}}</td>
                    <td>{{transaction.quantity}}</td>
                    <td>{{transaction.transactionDate}}</td>
                </tr>
            </tbody>
        </table>
        <div class="row">
            <div class="col-md-12">
                <ul class="pagination">
                    <li class="page-item" v-for="page in totalPage">
                        <button @click="changePage(page)">{{page}}</button>
                    </li>
                </ul>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import Vue from 'vue';
import Component from 'vue-class-component';
import { CartService, TransactionViewModel } from '../Services/NSwagService';
   

    @Component({
        created: async function (this: ViewTransaction) {
            await this.getTransactions();
        }
    })
    export default class ViewTransaction extends Vue {
        service: CartService = new CartService();
        transactionView: TransactionViewModel[] = [];

        //untuk filter dan paginate
        filterByName = '';
        pageIndex = 1;
        itemPerPage = 3;
        totalData = 1;
        totalPage = 1;

        async getTransactions(): Promise<void> {
            this.totalData = await this.service.getTotalData();
            this.totalPage = Math.ceil(this.totalData / this.itemPerPage);
            this.transactionView = await this.service.getFilterData(this.pageIndex, this.itemPerPage, this.filterByName);
        }

        async changePage(page: number): Promise<void> {
            this.pageIndex = page;
            await this.getTransactions();
        }
    }
</script>
