import moment from 'moment'

import backend from '../../config/backend.json'
import { BaseController } from '../BaseController'
import Dashboard from './Dashboard'

import { createData } from '../../helpers/AnalyticsHelper'
import { createData as createDataBilling } from '../../helpers/BillingHelper'
import { Hit, ActionResult, Invoice } from '../../entities/api'

export class DashboardController extends BaseController {
    private component?: Dashboard = undefined

    constructor(component: Dashboard) {
        super()
        this.component = component
    }

    initialize = () => {
        this.initializeHits()
        this.initializeInvoices()
    }

    initializeHits = () => {
        this.component!.setState({ hitsLoading: true })
        this.getHits(
            (json: ActionResult) => {
                let hits: any[] = []
                json.results.forEach((hit: Hit) => {
                    hits.push(createData(moment(hit.date!).toDate(), hit.ip!, hit.type!, hit.userAgent!, hit.price!))
                })
                this.component!.setState({ hits, hitsLoading: false })
            },
            (e: any) => {
                this.component!.setState({ hitsLoading: false, hitsError: true })
            }
        )
    }

    initializeInvoices = () => {
        this.component!.setState({ invoicesLoading: true })
        this.getInvoices(
            (json: ActionResult) => {
                let invoices: any[] = []
                json.results.forEach((invoice: Invoice) => {
                    invoices.push(createDataBilling(invoice.invoiceId!, invoice.number!, invoice.price!, invoice.paid!, moment(invoice.date!).toDate()))
                })
                this.component!.setState({ invoices, invoicesLoading: false })
            },
            (e: any) => {
                this.component!.setState({ invoicesLoading: false, invoicesError: true })
            }
        )
    }

    getHits = (success?: (json: any) => void, error?: (e: any) => void) => {
        this.callBackend(this.endpoint('/hits'), 'GET', success, error)
    }

    getHitsDaily = (success?: (json: any) => void, error?: (e: any) => void) => {
        this.callBackend(this.endpoint('/hits/daily'), 'GET', success, error)
    }

    getInvoices = (success?: (json: any) => void, error?: (e: any) => void) => {
        this.callBackend(this.endpoint('/invoices'), 'GET', success, error)
    }

    endpoint(path: string) {
        return this.culturedEndpoint(backend.dashboard.endpoint + path)
    }
}
