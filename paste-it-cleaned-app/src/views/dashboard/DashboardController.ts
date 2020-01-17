import moment from 'moment'

import backend from '../../config/backend.json'
import { BaseController } from '../BaseController'
import Dashboard from './Dashboard'

import { createData } from '../../helpers/AnalyticsHelper'
import { createData as createDataBilling } from '../../helpers/BillingHelper'
import { HitEntity, ActionResult, InvoiceEntity } from '../../entities/api'

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
        this.getHitsBackend(
            (json: ActionResult) => {
                let hits: any[] = []
                json.results.forEach((hit: HitEntity) => {
                    hits.push(createData(moment(hit.date!).toDate(), hit.ip!, hit.type!, hit.userAgent!, hit.price!))
                })
                this.component!.setState({ hits, hitsLoading: false, isLoaded: true })
            },
            (e: any) => {
                this.component!.setState({ hitsLoading: false, hitsError: true, isLoaded: true })
            }
        )
    }

    initializeInvoices = () => {
        this.component!.setState({ invoicesLoading: true })
        this.getInvoicesBackend(
            (json: ActionResult) => {
                let invoices: any[] = []
                json.results.forEach((invoice: InvoiceEntity) => {
                    invoices.push(createDataBilling(invoice.invoiceId!, invoice.number!, invoice.price!, invoice.paid!, moment(invoice.date!).toDate()))
                })
                this.component!.setState({ invoices, invoicesLoading: false, isLoaded: true })
            },
            (e: any) => {
                this.component!.setState({ invoicesLoading: false, invoicesError: true, isLoaded: true })
            }
        )
    }

    private getHitsBackend = (success?: (json: any) => void, error?: (e: any) => void) => {
        this.callBackend(this.endpoint('/hits'), 'GET', success, error)
    }

    private getHitsDailyBackend = (success?: (json: any) => void, error?: (e: any) => void) => {
        this.callBackend(this.endpoint('/hits/daily'), 'GET', success, error)
    }

    private getInvoicesBackend = (success?: (json: any) => void, error?: (e: any) => void) => {
        this.callBackend(this.endpoint('/invoices'), 'GET', success, error)
    }

    private endpoint(path: string) {
        return this.culturedEndpoint(backend.endpoints.dashboard + path)
    }
}
