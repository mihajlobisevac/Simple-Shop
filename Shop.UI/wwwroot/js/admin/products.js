var app = new Vue({
    el: '#app',
    data: {
        editing: false,
        loading: false,
        products: [],
        objectIndex: 0,
        productModel: {
            id: 0,
            name: "Name",
            description: "Description",
            value: 1.99
        }
    },
    mounted() {
        this.getProducts();
    },
    methods: {
        getProduct(id) {
            this.loading = true;
            axios.get('/api/product/' + id)
                .then(res => {
                    console.log(res);
                    var product = res.data;
                    this.productModel = {
                        id: product.id,
                        name: product.name,
                        description: product.description,
                        value: product.value
                    }
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                });
        },
        getProducts() {
            this.loading = true;
            axios.get('/api/product')
                .then(res => {
                    console.log(res);
                    this.products = res.data;
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                });
        },
        createProduct() {
            this.loading = true;
            axios.post('/api/product', this.productModel)
                .then(res => {
                    console.log(res.data);
                    this.products.push(res.data);
                    this.editing = false;
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                });
        },
        updateProduct() {
            this.loading = true;
            axios.put('/api/product', this.productModel)
                .then(res => {
                    console.log(res.data);
                    this.products.splice(this.objectIndex, 1, res.data);
                    this.editing = false;
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                });
        },
        deleteProduct(id, index) {
            this.loading = true;
            axios.delete('/api/product/' + id)
                .then(res => {
                    console.log(res);
                    this.products.splice(index, 1)
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                });
        },
        newProduct() {
            this.editing = true;
            this.productModel.id = 0;
        },
        editProduct(id, index) {
            this.objectIndex = index
            this.getProduct(id);
            this.editing = true;
        },
        cancel() {
            this.editing = false;
        }
    },
    computed: {
    }
});
