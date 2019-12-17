import moment from 'moment'
import React from 'react'

import PictureAsPdfIcon from '@material-ui/icons/PictureAsPdf'
import CheckCircleOutlineIcon from '@material-ui/icons/CheckCircleOutline'
import ErrorOutlineIcon from '@material-ui/icons/ErrorOutline'

export function createData(trxId: string, number: string, amount: number, status: 'paid' | 'pending', paidOn: Date) {
    var numberNode = <a href={`/billing/invoice/${number}`}>#{number}</a>
    var amountNode = amount.toString()
    var statusNode =
        status === 'paid' ? (
            <span className="billing-status-paid">
                <CheckCircleOutlineIcon /> Paid
            </span>
        ) : (
            <span className="billing-status-pending">
                <ErrorOutlineIcon /> Pending
            </span>
        )
    var paidOnNode = moment(paidOn).format('YYYY-MM-DD HH:MM:SS')
    var pdfNode = (
        <a href={`/billing/invoice/${number}`}>
            <PictureAsPdfIcon />
        </a>
    )
    return { key: trxId, number: numberNode, amount: amountNode, status: statusNode, paidon: paidOnNode, pdf: pdfNode }
}
