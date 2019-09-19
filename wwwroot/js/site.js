new Vue({
    el: "#vue-app",
    mounted: function() {
        //this.getNews()
    },
    methods: {
        getNews() {
            this.loading = true;
            axios.get("https://newsapi.org/v2/top-headlines?country=us&apiKey=apikey") // API key removed for source control and maybe change of direction.
            .then(response => {
                this.news = response.data.articles
                this.loading = false;
            })
        },
        setModal(news) {
            this.modal = news
        },  
        scrollTop() {
            $("html, body").animate({scrollTop:"0"}, 500)
        }
    },
    computed: {
        filtered() {
          return this.articles.filter(news => {
            return news.title.toLowerCase().includes(this.search.toLowerCase())
          })
        }
    },
    data: {
        news: "",
        search: "",
        modal: {},
        loading: true
    }
})