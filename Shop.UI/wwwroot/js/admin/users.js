var app = new Vue({
    el: '#app',
    data: {
        username: ""
    },
    mounted() {
        //todo: get all users
    },
    methods: {
        createUser() {
            axios.post('/api/users', { username: this.username })
                .then(res => {
                    console.log(res);
                })
                .catch(err => {
                    console.log(err)
                })
        }
    },
    computed: {
    }
});
