import moment from 'moment'

import backend from '../../../config/backend.json'
import { BaseController } from '../../BaseController'
import BillingInformation from './BillingInformation'

import { createData as createDataBilling } from '../../../helpers/BillingHelper'
import { ActionResult, InvoiceEntity } from '../../../entities/api'

export class BillingController extends BaseController {
    private component?: BillingInformation = undefined

    constructor(component: BillingInformation) {
        super()
        this.component = component
    }

    initialize = () => {
        this.initializeInvoices()
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

    showChangePaymentMethod = () => {
        this.component!.setState({
            isChangingMethod: true,
        })
    }

    showChangePaymentMethodToStripe = () => {
        this.component!.setState({
            isChangingMethod: false,
            isAddingStripe: true,
        })
    }

    showChangePaymentMethodToPayPal = () => {
        this.component!.setState({
            isChangingMethod: false,
            isAddingPayPal: true,
        })
    }

    private getInvoicesBackend = (success?: (json: any) => void, error?: (e: any) => void) => {
        this.callBackend(this.endpoint('/invoices'), 'GET', success, error)
    }

    private endpoint(path: string) {
        return this.culturedEndpoint(backend.endpoints.billing + path)
    }
}
