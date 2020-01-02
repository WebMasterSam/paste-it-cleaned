import backend from '../config/backend.json'
import { BaseController } from './BaseController'

export class DashboardController extends BaseController {
    getHits = (success?: (json: any) => void, error?: (e: any) => void) => {
        this.callBackend(this.endpoint('/hits'), 'GET', success, error)
    }

    getInvoices = (success?: (json: any) => void, error?: (e: any) => void) => {
        this.callBackend(this.endpoint('/invoices'), 'GET', success, error)
    }

    endpoint(path: string) {
        return this.culturedEndpoint(backend.dashboard.endpoint + path)
    }
}
