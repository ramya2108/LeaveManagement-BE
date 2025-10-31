import { create, router as _router, defaults } from 'json-server'
import { rewriter } from 'json-server'
import routes from './routes.json' assert { type: 'json' }

const server = create()
const router = _router('db.json')
const middlewares = defaults()

server.use(middlewares)

// custom routes can go here before router
server.get('/api/status', (req, res) => {
    res.json({ message: 'JSON Server is running' })
})

// mount router (this exposes /roles, /users, etc.)
server.use(router)
server.use(rewriter(routes))
server.use(router)

server.listen(3000, () => {
    console.log('JSON Server running on http://localhost:3000')
})